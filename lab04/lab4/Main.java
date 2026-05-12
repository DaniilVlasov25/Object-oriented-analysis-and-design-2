import javax.swing.*;
import javax.swing.table.DefaultTableModel;
import java.awt.*;
import java.sql.*;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;


class RealBankService {
    private Connection apiConnection;
    private String merchantKey = "SECRET_KEY_123";
    private String lastTransactionId;

    public boolean processPayment(int amount, String cardInfo) {
        try {
            System.out.println("[Bank] Соединение установлено: " + (apiConnection != null ? "Active" : "Default"));
            System.out.println("[Bank] Проверка MerchantKey: " + merchantKey);
            
            Thread.sleep(3000); 
            
            System.out.println("[Bank] Реальная транзакция на сумму " + amount + ". Карта: " + cardInfo);
            
            this.lastTransactionId = "REAL-" + java.util.UUID.randomUUID().toString().substring(0, 8);
            
            return true;
        } catch (InterruptedException e) {
            Thread.currentThread().interrupt();
            return false;
        }
    }

    public String getTransactionId() {
        return lastTransactionId;
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

    public int getId() { return id; }
    public String getName() { return name; }

    public String getFullInfo() {
        return "ID: " + id + " | " + name + " | " + email;
    }

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

    public int getId() { return id; }
    public String getName() { return name; }
    public int getPrice() { return price; }

    @Override
    public String toString() { return name + " (" + stockQuantity + " шт.) [" + price + " руб.]"; }

    public boolean isAvailable(int qty) {
        return this.stockQuantity >= qty;
    }
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

    public int getQuantity() { return quantity; }
    public int getUnitPrice() { return unitPrice; }
    public int getSubtotal() { return quantity * unitPrice; }
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

    public void setId(int id) { this.id = id; }

    public int calculateTotal() {
        this.totalAmount = items.stream().mapToInt(OrderItem::getSubtotal).sum();
        return this.totalAmount;
    }

    public void updateStatus(String newStatus) {
        this.status = newStatus;
        System.out.println("Заказ #" + id + " от " + date + " переведен в статус: " + status);
    }

   public boolean processPayment() {
    RealBankService bank = new RealBankService();
    if (bank.processPayment(calculateTotal(), "4444-5555-6666-7777")) {
        System.out.println("Оплата подтверждена банком. ID транзакции: " + bank.getTransactionId());
        return true;
    }
    return false;
}

    public void addOrderItem(Product p, int q) { items.add(new OrderItem(p, q)); }
    public Customer getCustomer() { return customer; }
    public List<OrderItem> getItems() { return items; }
    public int getTotalAmount() { return totalAmount; }
}

public class Main extends JFrame {
    private JComboBox<Customer> custCombo = new JComboBox<>();
    private JComboBox<Product> prodCombo = new JComboBox<>();
    private JSpinner qtySpin = new JSpinner(new SpinnerNumberModel(1, 1, 100, 1));
    private DefaultTableModel model;
    private JLabel totalLabel = new JLabel("Итого: 0 руб.");
    private Order currentOrder;

    private final String URL = "jdbc:mysql://localhost:3306/shop_db";
    private final String USER = "root";
    private final String PASS = "$53%Gfr64anti3okc7"; 

    public Main() {
        setTitle("Магазин");
        setSize(850, 500);
        setDefaultCloseOperation(EXIT_ON_CLOSE);
        setLayout(new BorderLayout(15, 15));

        loadDataFromDB();

        JPanel top = new JPanel();
        top.add(new JLabel("Клиент:")); top.add(custCombo);
        top.add(new JLabel("Товар:")); top.add(prodCombo);
        top.add(new JLabel("Кол-во:")); top.add(qtySpin);
        JButton addBtn = new JButton("Добавить");
        top.add(addBtn);
        add(top, BorderLayout.NORTH);

        model = new DefaultTableModel(new String[]{"Товар", "Цена", "Кол-во", "Сумма"}, 0);
        add(new JScrollPane(new JTable(model)), BorderLayout.CENTER);

        JPanel bottom = new JPanel(new BorderLayout());
        JButton payBtn = new JButton("Оплатить заказ");
        bottom.add(totalLabel, BorderLayout.WEST);
        bottom.add(payBtn, BorderLayout.EAST);
        add(bottom, BorderLayout.SOUTH);

        custCombo.addActionListener(e -> {
            Customer c = (Customer) custCombo.getSelectedItem();
            if (c != null) {
                currentOrder = new Order(c);
                model.setRowCount(0);
                totalLabel.setText("Итого: 0 руб.");
                System.out.println(c.getFullInfo());
            }
        });

        addBtn.addActionListener(e -> {
            Product p = (Product) prodCombo.getSelectedItem();
            int q = (int) qtySpin.getValue();
            if (p != null && p.isAvailable(q)) {
                // ДОБАВЬТЕ ЭТУ ПРОВЕРКУ:
                if (currentOrder == null) {
                    currentOrder = new Order((Customer) custCombo.getSelectedItem());
                }
        
                currentOrder.addOrderItem(p, q);
                model.addRow(new Object[]{p.getName(), p.getPrice(), q, p.getPrice() * q});
                totalLabel.setText("Итого: " + currentOrder.calculateTotal() + " руб.");
            }
        });

        payBtn.addActionListener(e -> {
            if (currentOrder == null || currentOrder.calculateTotal() == 0) return;
            if (currentOrder.getTotalAmount() > currentOrder.getCustomer().balance) {
                JOptionPane.showMessageDialog(this, "Недостаточно средств!");
                return;
            }

            payBtn.setEnabled(false);
            new Thread(() -> {
                if (currentOrder.processPayment()) {
                    saveToDB(currentOrder);
                    SwingUtilities.invokeLater(() -> {
                        JOptionPane.showMessageDialog(this, "Заказ успешно оплачен!");
                        loadDataFromDB();
                        payBtn.setEnabled(true);
                        currentOrder = new Order((Customer) custCombo.getSelectedItem());
                        model.setRowCount(0);
                        totalLabel.setText("Итого: 0 руб.");
                    });
                }
            }).start();
        });

        setLocationRelativeTo(null);
    }

    private void loadDataFromDB() {
        try (Connection conn = DriverManager.getConnection(URL, USER, PASS)) {
            custCombo.removeAllItems();
            ResultSet rc = conn.createStatement().executeQuery("SELECT * FROM customer");
            while (rc.next()) {
                custCombo.addItem(new Customer(rc.getInt("id"), rc.getString("name"), rc.getString("email"), rc.getInt("balance")));
            }
            
            prodCombo.removeAllItems();
            ResultSet rp = conn.createStatement().executeQuery("SELECT * FROM product");
            while (rp.next()) {
                prodCombo.addItem(new Product(rp.getInt("id"), rp.getString("name"), rp.getInt("price"), rp.getInt("stockQuantity")));
            }
        } catch (SQLException e) { 
            JOptionPane.showMessageDialog(this, "Ошибка БД: " + e.getMessage());
        }
    }

    private void saveToDB(Order order) {
        try (Connection conn = DriverManager.getConnection(URL, USER, PASS)) {
            conn.setAutoCommit(false);
            try {
                String sqlOrder = "INSERT INTO orders (customer_id, order_date, status, totalAmount) VALUES (?, NOW(), 'PAID', ?)";
                PreparedStatement psO = conn.prepareStatement(sqlOrder, Statement.RETURN_GENERATED_KEYS);
                psO.setInt(1, order.getCustomer().getId());
                psO.setInt(2, order.calculateTotal());
                psO.executeUpdate();

                ResultSet rs = psO.getGeneratedKeys();
                if (rs.next()) {
                    int generatedId = rs.getInt(1);
                    order.setId(generatedId); 
                    order.updateStatus("PAID");

                    String sqlItem = "INSERT INTO orderitem (order_id, product_id, quantity, unitPrice) VALUES (?, ?, ?, ?)";
                    PreparedStatement psI = conn.prepareStatement(sqlItem);
                
                    String sqlProduct = "UPDATE product SET stockQuantity = stockQuantity - ? WHERE id = ?";
                    PreparedStatement psP = conn.prepareStatement(sqlProduct);

                    for (OrderItem item : order.getItems()) {
                        psI.setInt(1, generatedId);
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

                    String sqlBalance = "UPDATE customer SET balance = balance - ? WHERE id = ?";
                    PreparedStatement psB = conn.prepareStatement(sqlBalance);
                    psB.setInt(1, order.calculateTotal());
                    psB.setInt(2, order.getCustomer().getId());
                    psB.executeUpdate();
                
                    conn.commit();
                }
            } catch (SQLException ex) {
                conn.rollback();
                throw ex;
            }
        } catch (SQLException e) { 
            e.printStackTrace();
            JOptionPane.showMessageDialog(this, "Ошибка при сохранении в БД: " + e.getMessage());
        }
    }

    public static void main(String[] args) {
        SwingUtilities.invokeLater(() -> new Main().setVisible(true));
    }
}