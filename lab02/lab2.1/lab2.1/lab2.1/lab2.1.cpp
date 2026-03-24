#include "framework.h"
#include "resource.h"
#define MAX_LOADSTRING 100
#include <string>
#include <vector>
#include <sstream>

// Интерфейс реализации
class Damage {
public:
    virtual int calculateDamage(int basePower) = 0;
    virtual std::string getDamageType() = 0;
    virtual ~Damage() {}
};

// Конкретная реализация: Физический урон
class PhysicalDamage : public Damage {
public:
    int calculateDamage(int basePower) override {
        return basePower * 2;  // Физический: база × 2
    }

    std::string getDamageType() override {
        return "Physical";
    }
};

// Конкретная реализация: Магический урон
class MagicDamage : public Damage {
public:
    int calculateDamage(int basePower) override {
        return basePower * 3 + 10;  // Магический: база × 3 + 10
    }

    std::string getDamageType() override {
        return "Magic";
    }
};

// Конкретная реализация: Огненный урон
class FireDamage : public Damage {
public:
    int calculateDamage(int basePower) override {
        return basePower * 2 + 5;  // Огненный: база × 2 + 5
    }

    std::string getDamageType() override {
        return "Fire";
    }
};

// Абстрактный класс (Abstraction)
class Weapon {
protected:
    std::string name;
    int durability;
    int basePower;
    Damage* damageType;

public:
    Weapon(Damage* damage, std::string n, int dur, int power)
        : damageType(damage), name(n), durability(dur), basePower(power) {
    }

    virtual int attack() = 0;
    virtual std::string getDescription() = 0;
    virtual ~Weapon() {
        delete damageType;
    }

    void setDamageType(Damage* newDamage) {
        delete damageType;
        damageType = newDamage;
    }

    std::string getName() { return name; }
    int getDurability() { return durability; }
};

// Меч
class Sword : public Weapon {
private:
    int sharpness;

public:
    Sword(Damage* damage, int sharp = 10)
        : Weapon(damage, "Sword", 100, 50), sharpness(sharp) {
    }

    int attack() override {
        int totalPower = basePower + sharpness;
        return damageType->calculateDamage(totalPower); 
    }

    std::string getDescription() override {
        return name + " (" + damageType->getDamageType() + ")";
    }
};

// Лук
class Bow : public Weapon {
private:
    int range;

public:
    Bow(Damage* damage, int rng = 15)
        : Weapon(damage, "Bow", 80, 30), range(rng) {
    }

    int attack() override {
        int totalPower = basePower + range;
        return damageType->calculateDamage(totalPower);  
    }

    std::string getDescription() override {
        return name + " (" + damageType->getDamageType() + ")";
    }
};

// Посох
class Staff : public Weapon {
private:
    int manaCost;

public:
    Staff(Damage* damage, int mana = 20)
        : Weapon(damage, "Staff", 60, 40), manaCost(mana) {
    }

    int attack() override {
        int totalPower = basePower + manaCost;
        return damageType->calculateDamage(totalPower); 
    }

    std::string getDescription() override {
        return name + " (" + damageType->getDamageType() + ")";
    }
};

// Игрок
class Player {
private:
    std::vector<Weapon*> inventory;

public:
    bool equipWeapon(Weapon* weapon) {
        if (weapon == nullptr) return false;
        inventory.push_back(weapon);
        return true;
    }

    int useWeapon(int index) {
        if (index >= 0 && index < (int)inventory.size()) {
            return inventory[index]->attack();
        }
        return -1;
    }

    void clearInventory() {
        for (auto weapon : inventory) {
            delete weapon;
        }
        inventory.clear();
    }

    std::vector<Weapon*>& getInventory() { return inventory; }

    size_t getInventorySize() { return inventory.size(); }

    ~Player() {
        clearInventory();
    }
};

// ГЛОБАЛЬНЫЕ ПЕРЕМЕННЫЕ WINDOWS 

HINSTANCE hInst;
WCHAR szTitle[MAX_LOADSTRING];
WCHAR szWindowClass[MAX_LOADSTRING];

// Игрок
Player player;
int selectedWeaponType = 0;  // 0=Sword, 1=Bow, 2=Staff
int selectedDamageType = 0;  // 0=Physical, 1=Magic, 2=Fire
std::wstring attackResult = L"Оружие не создано";

// Элементы управления
HWND hComboWeaponType;
HWND hComboDamageType;
HWND hBtnCreate;
HWND hBtnAttack;
HWND hComboInventory;
HWND hBtnClear;
HWND hLblResult;
HWND hLblInfo;

// ОБЪЯВЛЕНИЯ ФУНКЦИЙ

ATOM                MyRegisterClass(HINSTANCE hInstance);
BOOL                InitInstance(HINSTANCE, int);
LRESULT CALLBACK    WndProc(HWND, UINT, WPARAM, LPARAM);
INT_PTR CALLBACK    About(HWND, UINT, WPARAM, LPARAM);
void CreateWeapon();
void Attack();
void ClearInventory();
void UpdateInventoryCombo();
void UpdateInfoLabel();

// ТОЧКА ВХОДА

int APIENTRY wWinMain(_In_ HINSTANCE hInstance,
    _In_opt_ HINSTANCE hPrevInstance,
    _In_ LPWSTR    lpCmdLine,
    _In_ int       nCmdShow)
{
    UNREFERENCED_PARAMETER(hPrevInstance);
    UNREFERENCED_PARAMETER(lpCmdLine);

    LoadStringW(hInstance, IDS_APP_TITLE, szTitle, MAX_LOADSTRING);
    LoadStringW(hInstance, IDC_LAB21, szWindowClass, MAX_LOADSTRING);
    MyRegisterClass(hInstance);

    if (!InitInstance(hInstance, nCmdShow))
        return FALSE;

    HACCEL hAccelTable = LoadAccelerators(hInstance, MAKEINTRESOURCE(IDC_LAB21));
    MSG msg;

    while (GetMessage(&msg, nullptr, 0, 0))
    {
        if (!TranslateAccelerator(msg.hwnd, hAccelTable, &msg))
        {
            TranslateMessage(&msg);
            DispatchMessage(&msg);
        }
    }

    return (int)msg.wParam;
}

// РЕГИСТРАЦИЯ КЛАССА ОКНА

ATOM MyRegisterClass(HINSTANCE hInstance)
{
    WNDCLASSEXW wcex;
    wcex.cbSize = sizeof(WNDCLASSEX);
    wcex.style = CS_HREDRAW | CS_VREDRAW;
    wcex.lpfnWndProc = WndProc;
    wcex.cbClsExtra = 0;
    wcex.cbWndExtra = 0;
    wcex.hInstance = hInstance;
    wcex.hIcon = LoadIcon(nullptr, IDI_APPLICATION);
    wcex.hCursor = LoadCursor(nullptr, IDC_ARROW);
    wcex.hbrBackground = (HBRUSH)(COLOR_WINDOW + 1);
    wcex.lpszMenuName = nullptr;
    wcex.lpszClassName = szWindowClass;
    wcex.hIconSm = LoadIcon(nullptr, IDI_APPLICATION);

    return RegisterClassExW(&wcex);
}

// ИНИЦИАЛИЗАЦИЯ ПРИЛОЖЕНИЯ

BOOL InitInstance(HINSTANCE hInstance, int nCmdShow)
{
    hInst = hInstance;

    HWND hWnd = CreateWindowW(szWindowClass, L"RPG Weapon System",
        WS_OVERLAPPEDWINDOW, CW_USEDEFAULT, 0, 600, 550, nullptr, nullptr, hInstance, nullptr);

    if (!hWnd)
    {
        return FALSE;
    }

    ShowWindow(hWnd, nCmdShow);
    UpdateWindow(hWnd);

    return TRUE;
}

// СОЗДАНИЕ ОРУЖИЯ

void CreateWeapon() {
    Weapon* weapon = nullptr;
    Damage* damage = nullptr;

    // 1. Создаём тип урона (Реализация)
    if (selectedDamageType == 0)
        damage = new PhysicalDamage();
    else if (selectedDamageType == 1)
        damage = new MagicDamage();
    else if (selectedDamageType == 2)
        damage = new FireDamage();

    // 2. Создаём тип оружия (Абстракция)
    if (selectedWeaponType == 0)
        weapon = new Sword(damage);
    else if (selectedWeaponType == 1)
        weapon = new Bow(damage);
    else if (selectedWeaponType == 2)
        weapon = new Staff(damage);

    if (weapon && player.equipWeapon(weapon)) {
        std::string desc = weapon->getDescription();
        attackResult = L"✓ Создано: " + std::wstring(desc.begin(), desc.end());
        SetWindowTextW(hLblResult, attackResult.c_str());
    }

    UpdateInventoryCombo();
    UpdateInfoLabel();
}

// АТАКА

void Attack() {
    int selectedIndex = (int)SendMessageW(hComboInventory, CB_GETCURSEL, 0, 0);

    if (selectedIndex == CB_ERR || player.getInventorySize() == 0) {
        attackResult = L"❌ Выберите оружие из инвентаря!";
        SetWindowTextW(hLblResult, attackResult.c_str());
        return;
    }

    int damage = player.useWeapon(selectedIndex);
    auto& inventory = player.getInventory();
    std::string desc = inventory[selectedIndex]->getDescription();

    attackResult = L"⚔ " + std::wstring(desc.begin(), desc.end()) +
        L" наносит " + std::to_wstring(damage) + L" урона!";

    SetWindowTextW(hLblResult, attackResult.c_str());
}

// ОЧИСТКА ИНВЕНТАРЯ

void ClearInventory() {
    player.clearInventory();

    SendMessageW(hComboInventory, CB_RESETCONTENT, 0, 0);
    SendMessageW(hComboInventory, CB_ADDSTRING, 0, (LPARAM)L"Инвентарь пуст");
    EnableWindow(hComboInventory, FALSE);

    attackResult = L"🗑 Инвентарь очищен!";
    SetWindowTextW(hLblResult, attackResult.c_str());
    UpdateInfoLabel();
}

// ОБНОВЛЕНИЕ COMBOBOX ИНВЕНТАРЯ

void UpdateInventoryCombo() {
    SendMessageW(hComboInventory, CB_RESETCONTENT, 0, 0);

    auto& inventory = player.getInventory();

    if (inventory.empty()) {
        SendMessageW(hComboInventory, CB_ADDSTRING, 0, (LPARAM)L"Инвентарь пуст");
        EnableWindow(hComboInventory, FALSE);
    }
    else {
        EnableWindow(hComboInventory, TRUE);
        int index = 1;
        for (auto weapon : inventory) {
            std::string desc = weapon->getDescription();
            std::wstring itemText = L"[" + std::to_wstring(index++) + L"] " +
                std::wstring(desc.begin(), desc.end());
            SendMessageW(hComboInventory, CB_ADDSTRING, 0, (LPARAM)itemText.c_str());
        }
        SendMessageW(hComboInventory, CB_SETCURSEL, inventory.size() - 1, 0);
    }
}

// ОБНОВЛЕНИЕ ИНФОРМАЦИИ

void UpdateInfoLabel() {
    std::wstringstream ss;
    ss << L"В инвентаре: " << player.getInventorySize() << L" оружие(я)";
    SetWindowTextW(hLblInfo, ss.str().c_str());
}

// ОБРАБОТЧИК ОКНА

LRESULT CALLBACK WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
{
    switch (message)
    {
    case WM_CREATE:
        CreateWindowW(L"STATIC", L"СИСТЕМА КОНФИГУРАЦИИ ОРУЖИЯ",
            WS_VISIBLE | WS_CHILD | WS_BORDER | SS_CENTER,
            20, 10, 520, 30, hWnd, nullptr, hInst, nullptr);

        CreateWindowW(L"STATIC", L"Тип оружия:",
            WS_VISIBLE | WS_CHILD, 20, 55, 200, 25, hWnd, nullptr, hInst, nullptr);

        hComboWeaponType = CreateWindowW(L"COMBOBOX", L"",
            WS_VISIBLE | WS_CHILD | WS_BORDER | CBS_DROPDOWNLIST | WS_VSCROLL,
            20, 80, 200, 100, hWnd, nullptr, hInst, nullptr);
        SendMessageW(hComboWeaponType, CB_ADDSTRING, 0, (LPARAM)L"Меч (Sword)");
        SendMessageW(hComboWeaponType, CB_ADDSTRING, 0, (LPARAM)L"Лук (Bow)");
        SendMessageW(hComboWeaponType, CB_ADDSTRING, 0, (LPARAM)L"Посох (Staff)");
        SendMessageW(hComboWeaponType, CB_SETCURSEL, 0, 0);

        CreateWindowW(L"STATIC", L"Тип урона:",
            WS_VISIBLE | WS_CHILD, 20, 115, 200, 25, hWnd, nullptr, hInst, nullptr);

        hComboDamageType = CreateWindowW(L"COMBOBOX", L"",
            WS_VISIBLE | WS_CHILD | WS_BORDER | CBS_DROPDOWNLIST | WS_VSCROLL,
            20, 140, 200, 100, hWnd, nullptr, hInst, nullptr);
        SendMessageW(hComboDamageType, CB_ADDSTRING, 0, (LPARAM)L"Физический");
        SendMessageW(hComboDamageType, CB_ADDSTRING, 0, (LPARAM)L"Магический");
        SendMessageW(hComboDamageType, CB_ADDSTRING, 0, (LPARAM)L"Огненный");
        SendMessageW(hComboDamageType, CB_SETCURSEL, 0, 0);

        hBtnCreate = CreateWindowW(L"BUTTON", L"Создать оружие",
            WS_VISIBLE | WS_CHILD | BS_PUSHBUTTON,
            20, 185, 200, 35, hWnd, (HMENU)1001, hInst, nullptr);

        CreateWindowW(L"STATIC", L"Выберите оружие для атаки:",
            WS_VISIBLE | WS_CHILD, 20, 230, 200, 25, hWnd, nullptr, hInst, nullptr);

        hComboInventory = CreateWindowW(L"COMBOBOX", L"",
            WS_VISIBLE | WS_CHILD | WS_BORDER | CBS_DROPDOWNLIST | WS_VSCROLL,
            20, 255, 350, 100, hWnd, nullptr, hInst, nullptr);
        SendMessageW(hComboInventory, CB_ADDSTRING, 0, (LPARAM)L"Инвентарь пуст");
        SendMessageW(hComboInventory, CB_SETCURSEL, 0, 0);
        EnableWindow(hComboInventory, FALSE);

        hBtnAttack = CreateWindowW(L"BUTTON", L"Атаковать!",
            WS_VISIBLE | WS_CHILD | BS_PUSHBUTTON,
            20, 300, 165, 35, hWnd, (HMENU)1002, hInst, nullptr);

        hBtnClear = CreateWindowW(L"BUTTON", L"Очистить инвентарь",
            WS_VISIBLE | WS_CHILD | BS_PUSHBUTTON,
            205, 300, 165, 35, hWnd, (HMENU)1003, hInst, nullptr);

        hLblInfo = CreateWindowW(L"STATIC", L"В инвентаре: 0 оружие(я)",
            WS_VISIBLE | WS_CHILD | SS_CENTER | WS_BORDER,
            20, 350, 520, 25, hWnd, nullptr, hInst, nullptr);

        hLblResult = CreateWindowW(L"STATIC", attackResult.c_str(),
            WS_VISIBLE | WS_CHILD | SS_CENTER | WS_BORDER,
            20, 390, 520, 50, hWnd, nullptr, hInst, nullptr);
        break;

    case WM_COMMAND:
        if (HIWORD(wParam) == CBN_SELCHANGE) {
            if ((HWND)lParam == hComboWeaponType)
                selectedWeaponType = (int)SendMessageW(hComboWeaponType, CB_GETCURSEL, 0, 0);
            else if ((HWND)lParam == hComboDamageType)
                selectedDamageType = (int)SendMessageW(hComboDamageType, CB_GETCURSEL, 0, 0);
        }

        if (LOWORD(wParam) == 1001)
            CreateWeapon();
        if (LOWORD(wParam) == 1002)
            Attack();
        if (LOWORD(wParam) == 1003)
            ClearInventory();
        if (LOWORD(wParam) == IDM_ABOUT)
            DialogBox(hInst, MAKEINTRESOURCE(IDD_ABOUTBOX), hWnd, About);
        if (LOWORD(wParam) == IDM_EXIT)
            DestroyWindow(hWnd);
        break;

    case WM_PAINT:
    {
        PAINTSTRUCT ps;
        HDC hdc = BeginPaint(hWnd, &ps);
        EndPaint(hWnd, &ps);
    }
    break;

    case WM_DESTROY:
        PostQuitMessage(0);
        break;

    default:
        return DefWindowProc(hWnd, message, wParam, lParam);
    }
    return 0;
}

//  ОКНО О ПРОГРАММЕ

INT_PTR CALLBACK About(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
{
    UNREFERENCED_PARAMETER(lParam);
    switch (message)
    {
    case WM_INITDIALOG:
        return (INT_PTR)TRUE;

    case WM_COMMAND:
        if (LOWORD(wParam) == IDOK || LOWORD(wParam) == IDCANCEL)
        {
            EndDialog(hDlg, LOWORD(wParam));
            return (INT_PTR)TRUE;
        }
        break;
    }
    return (INT_PTR)FALSE;
}