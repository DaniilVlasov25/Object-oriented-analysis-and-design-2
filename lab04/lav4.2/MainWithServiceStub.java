import javax.swing.*;
import javax.swing.table.DefaultTableModel;
import java.awt.*;
import java.sql.*;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;
import java.util.UUID;


interface IPaymentGateway {
    boolean processPayment(int amount, String cardInfo);
    String getTransactionId();
}

class PaymentServiceStub implements IPaymentGateway {
    private boolean simulatedResponse = true;
    private String lastTransactionId;

    @Override
    public boolean processPayment(int amount, String cardInfo) {
        System.out.println("[Stub] Имитация платежа на сумму " + amount + ". Ответ: " + simulatedResponse);
        this.lastTransactionId = "STUB-ID-" + UUID.randomUUID().toString().substring(0, 5);
        return simulatedResponse;
    }

    @Override
    public String getTransactionId() { return lastTransactionId; }
}

class RealBankService implements IPaymentGateway {
    private String merchantKey = "SECRET_KEY_123"; 
    private String lastTransactionId;
    
    private Connection apiConnection; 

    @Override
    public boolean processPayment(int amount, String cardInfo) {
        try {
            System.out.println("[Bank] Соединение установлено: " + (apiConnection != null ? "Active" : "Default"));
            System.out.println("[Bank] Проверка MerchantKey: " + merchantKey);
            
            Thread.sleep(3000);
            
            System.out.println("[Bank] Реальная транзакция на сумму " + amount + ". Карта: " + cardInfo);
            this.lastTransactionId = "REAL-" + UUID.randomUUID().toString().substring(0, 8);
            return true;
        } catch (InterruptedException e) {
            Thread.currentThread().interrupt();
            return false;
        }
    }

    @Override
    public String getTransactionId() { 
        return lastTransactionId; 
    }
    
    public void setApiConnection(Connection conn) {
        this.apiConnection = conn;
    }
}

class Customer {
    private int id;
    private String name;
    private String email;
    public int balance;

    public Customer(int id, String name, String email, int balance) {
        this.id = id; this.name = name; this.email = email; this.balance = balance;
    }

    public String getFullInfo() {
        return "ID: " + id + " | Email: " + email + " | Balance: " + balance;
    }

    public void register(String email) {
        this.email = email;
    }

    public int getId() { return id; }
    @Override
    public String toString() { return name + " (" + balance + " руб.)"; }
}

class Product {
    private int id;
    private String name;
    private int price;
    private int stockQuantity;

    public Product(int id, String name, int price, int stockQuantity) {
        this.id = id; this.name = name; this.price = price; this.stockQuantity = stockQuantity;
    }

    public int getPrice() { return price; }
    
    public boolean isAvailable(int qty) {
        return this.stockQuantity >= qty;
    }

    public int getId() { return id; }
    public String getName() { return name; }

    @Override
    public String toString() { return name + " (" + stockQuantity + " шт.) [" + price + " руб.]"; }
}

class OrderItem {
    private int quantity;
    private int unitPrice;
    public Product product;

    public OrderItem(Product product, int quantity) {
        this.product = product;
        this.quantity = quantity;
        this.unitPrice = product.getPrice();
    }

    public int getSubtotal() {
        return quantity * unitPrice;
    }

    public int getQuantity() { return quantity; }
    public int getUnitPrice() { return unitPrice; }
}

class Order {
    private int id; 
    private Date date;
    private String status; 
    private int totalAmount; 
    private Customer customer;
    private List<OrderItem> items = new ArrayList<>();

    public Order(Customer customer) {
        this.customer = customer;
        this.date = new Date();
        this.status = "NEW";
    }

    public int calculateTotal() { 
        this.totalAmount = items.stream().mapToInt(OrderItem::getSubtotal).sum();
        return this.totalAmount;
    }

    public boolean processPayment(IPaymentGateway gateway) { 
        return gateway.processPayment(calculateTotal(), "4444-5555-6666-7777");
    }

    public void updateStatus(String newStatus) { 
        this.status = newStatus;
        System.out.println("Заказ #" + id + " от " + date.toString() + " сменил статус на: " + status);
    }

    public void setId(int id) { this.id = id; }
    public void addOrderItem(Product p, int q) { items.add(new OrderItem(p, q)); }
    public Customer getCustomer() { return customer; }
    public List<OrderItem> getItems() { return items; }
}


public class MainWithServiceStub extends JFrame {
    private JComboBox<Customer> custCombo = new JComboBox<>();
    private JComboBox<Product> prodCombo = new JComboBox<>();
    private JSpinner qtySpinner = new JSpinner(new SpinnerNumberModel(1, 1, 100, 1));
    private JCheckBox useStubCheck = new JCheckBox("Использовать Stub", true);
    private DefaultTableModel model;
    private JLabel totalLabel = new JLabel("Итого: 0 руб.");
    private Order currentOrder;

    private final String URL = "jdbc:mysql://localhost:3306/shop_db";
    private final String USER = "root";
    private final String PASS = "$53%Gfr64anti3okc7"; 

    public MainWithServiceStub() {
        setTitle("Магазин");
        setSize(1000, 550);
        setDefaultCloseOperation(EXIT_ON_CLOSE);
        setLayout(new BorderLayout(15, 15));

        loadDataFromDB();

        JPanel top = new JPanel(new FlowLayout(FlowLayout.LEFT));
        top.add(new JLabel("Клиент:")); top.add(custCombo);
        top.add(new JLabel("Товар:")); top.add(prodCombo);
        top.add(new JLabel("Кол-во:")); top.add(qtySpinner);
        JButton addBtn = new JButton("Добавить");
        top.add(addBtn); top.add(useStubCheck);
        add(top, BorderLayout.NORTH);

        model = new DefaultTableModel(new String[]{"Товар", "Цена", "Кол-во", "Сумма"}, 0);
        add(new JScrollPane(new JTable(model)), BorderLayout.CENTER);

        JPanel bottom = new JPanel(new BorderLayout());
        JButton payBtn = new JButton("Оплатить заказ");
        bottom.add(totalLabel, BorderLayout.WEST);
        bottom.add(payBtn, BorderLayout.EAST);
        add(bottom, BorderLayout.SOUTH);

        custCombo.addActionListener(e -> {
            Customer selected = (Customer) custCombo.getSelectedItem();
            if (selected != null) {
                currentOrder = new Order(selected);
                model.setRowCount(0);
                totalLabel.setText("Итого: 0 руб.");
                System.out.println("Выбран новый клиент: " + selected.getFullInfo()); 
            }
        });

        addBtn.addActionListener(e -> {
            Product p = (Product) prodCombo.getSelectedItem();
            int qty = (int) qtySpinner.getValue();
            
            if (p != null) {
                if (p.isAvailable(qty)) {
                    if (currentOrder == null) currentOrder = new Order((Customer) custCombo.getSelectedItem());
                    
                    currentOrder.addOrderItem(p, qty);
                    model.addRow(new Object[]{p.getName(), p.getPrice(), qty, p.getPrice() * qty});
                    totalLabel.setText("Итого: " + currentOrder.calculateTotal() + " руб.");
                } else {
                    JOptionPane.showMessageDialog(this, "Недостаточно товара на складе! Доступно: " + p.toString());
                }
            }
        });

        payBtn.addActionListener(e -> {
            if (currentOrder == null || currentOrder.calculateTotal() == 0) {
                JOptionPane.showMessageDialog(this, "Сначала добавьте товары в корзину!");
                return;
            }

            int total = currentOrder.calculateTotal();
            Customer buyer = currentOrder.getCustomer();

            if (!useStubCheck.isSelected() && buyer.balance < total) {
                JOptionPane.showMessageDialog(this, "Недостаточно средств для реальной оплаты!");
                return;
            }

            payBtn.setEnabled(false);
            new Thread(() -> {
                try (Connection conn = DriverManager.getConnection(URL, USER, PASS)) {
                    IPaymentGateway gateway = useStubCheck.isSelected() ? 
                        new PaymentServiceStub() : new RealBankService();

                    if (currentOrder.processPayment(gateway)) {
                        if (!useStubCheck.isSelected()) {
                            saveToDB(currentOrder, conn);
                            buyer.balance -= total; 
                        }

                        SwingUtilities.invokeLater(() -> {
                            JOptionPane.showMessageDialog(this, "Успешно! ID: " + gateway.getTransactionId());
                            loadDataFromDB();
                            model.setRowCount(0);
                            currentOrder = new Order((Customer) custCombo.getSelectedItem()); 
                            totalLabel.setText("Итого: 0 руб.");
                            qtySpinner.setValue(1);
                            payBtn.setEnabled(true);
                        });
                    }
                } catch (SQLException ex) { ex.printStackTrace(); }
            }).start();
        });

        setLocationRelativeTo(null);
    }

    private void loadDataFromDB() {
        try (Connection conn = DriverManager.getConnection(URL, USER, PASS)) {
            custCombo.removeAllItems();
            ResultSet rc = conn.createStatement().executeQuery("SELECT * FROM customer");
            while (rc.next()) custCombo.addItem(new Customer(rc.getInt("id"), rc.getString("name"), rc.getString("email"), rc.getInt("balance")));
            
            prodCombo.removeAllItems();
            ResultSet rp = conn.createStatement().executeQuery("SELECT * FROM product");
            while (rp.next()) prodCombo.addItem(new Product(rp.getInt("id"), rp.getString("name"), rp.getInt("price"), rp.getInt("stockQuantity")));
        } catch (SQLException e) { e.printStackTrace(); }
    }

    private void saveToDB(Order order, Connection conn) throws SQLException {
        conn.setAutoCommit(false);
        try {
            String sqlO = "INSERT INTO orders (customer_id, order_date, status, totalAmount) VALUES (?, NOW(), 'PAID', ?)";
            PreparedStatement psO = conn.prepareStatement(sqlO, Statement.RETURN_GENERATED_KEYS);
            psO.setInt(1, order.getCustomer().getId());
            psO.setInt(2, order.calculateTotal());
            psO.executeUpdate();

            ResultSet rs = psO.getGeneratedKeys();
            if (rs.next()) {
                int oId = rs.getInt(1);
                order.setId(oId);
                order.updateStatus("PAID"); 

                String sqlI = "INSERT INTO orderitem (order_id, product_id, quantity, unitPrice) VALUES (?, ?, ?, ?)";
                PreparedStatement psI = conn.prepareStatement(sqlI);
                
                String sqlP = "UPDATE product SET stockQuantity = stockQuantity - ? WHERE id = ?";
                PreparedStatement psP = conn.prepareStatement(sqlP);

                for (OrderItem item : order.getItems()) {
                    psI.setInt(1, oId);
                    psI.setInt(2, item.product.getId());
                    psI.setInt(3, item.getQuantity());
                    psI.setInt(4, item.getUnitPrice());
                    psI.addBatch();

                    psP.setInt(1, item.getQuantity());
                    psP.setInt(2, item.product.getId());
                    psP.addBatch();
                }
                psI.executeBatch();
                psP.executeBatch();

                String sqlU = "UPDATE customer SET balance = balance - ? WHERE id = ?";
                PreparedStatement psU = conn.prepareStatement(sqlU);
                psU.setInt(1, order.calculateTotal());
                psU.setInt(2, order.getCustomer().getId());
                psU.executeUpdate();
            }
            conn.commit();
        } catch (SQLException e) { conn.rollback(); throw e; }
    }

    public static void main(String[] args) {
        SwingUtilities.invokeLater(() -> new MainWithServiceStub().setVisible(true));
    }
}