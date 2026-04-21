import tkinter as tk
from tkinter import ttk, messagebox
from abc import ABC, abstractmethod
from typing import List


class Subject(ABC):
    """Абстрактный класс Издателя"""
    def __init__(self):
        self._observers: List['Observer'] = []
    
    def attach(self, observer: 'Observer') -> None:
        """Подписка наблюдателя на события"""
        if observer not in self._observers:
            self._observers.append(observer)
            print(f"[Subject] Подписан наблюдатель: {observer.__class__.__name__}")
    
    def detach(self, observer: 'Observer') -> None:
        """Отписка наблюдателя от событий"""
        if observer in self._observers:
            self._observers.remove(observer)
            print(f"[Subject] Отписан наблюдатель: {observer.__class__.__name__}")
    
    def notify(self) -> None:
        """Оповещение всех подписчиков"""
        print(f"[Subject] Оповещение {len(self._observers)} наблюдателей...")
        for observer in self._observers:
            observer.update(self)


class Observer(ABC):
    """Абстрактный класс Подписчика"""
    @abstractmethod
    def update(self, subject: Subject) -> None:
        pass


#Конкретный издатель
class GameCharacter(Subject):
    """Класс персонажа"""
    def __init__(self):
        super().__init__()
        
        # Атрибуты состояния
        self._health = 100
        self._maxHealth = 100
        self._mana = 50
        self._maxMana = 50
        self._gold = 0
        self._experience = 0
        self._level = 1
        self._is_alive = True

    # Операции изменения состояния
    def takeDamage(self, amount: int) -> None:
        if not self._is_alive:
            return
        self._health = max(0, self._health - amount)
        if self._health == 0:
            self._is_alive = False
        self.notify()

    def heal(self, amount: int) -> None:
        if not self._is_alive:
            return
        self._health = min(self._maxHealth, self._health + amount)
        self.notify()

    def gainGold(self, amount: int) -> None:
        if not self._is_alive:
            return
        self._gold += amount
        self.notify()

    def gainXP(self, amount: int) -> None:
        if not self._is_alive:
            return
        self._experience += amount
        while self._experience >= self._level * 100:
            self._experience -= self._level * 100
            self._level += 1
        self.notify()

    def restoreMana(self, amount: int) -> None:
        """Восстановить ману"""
        if not self._is_alive:
            return
        self._mana = min(self._maxMana, self._mana + amount)
        self.notify()

    def spendMana(self, amount: int) -> bool:
        """Потратить ману"""
        if not self._is_alive:
            return False
        if self._mana >= amount:
            self._mana -= amount
            self.notify()
            return True
        return False

    def castSpell(self, spellCost: int) -> bool:
        """Произнести заклинание"""
        if not self._is_alive:
            return False
        return self.spendMana(spellCost)

    def getHealth(self) -> int:
        return self._health

    def getMaxHealth(self) -> int:
        return self._maxHealth

    def getMana(self) -> int:
        return self._mana

    def getMaxMana(self) -> int:
        return self._maxMana

    def getGold(self) -> int:
        return self._gold

    def getXP(self) -> int:
        return self._experience

    def getLevel(self) -> int:
        return self._level

    def isAlive(self) -> bool:
        return self._is_alive


# Конкретные подписчики
class HealthBarUI(Observer):
    """Полоска здоровья"""
    def __init__(self, parent):
        self.frame = tk.Frame(parent)
        self.frame.pack(pady=5)
        
        tk.Label(self.frame, text="Здоровье:").pack()
        self.progressBar = ttk.Progressbar(self.frame, length=300, mode='determinate')
        self.progressBar.pack(pady=5)
        self.label = tk.Label(self.frame, text="HP: 100/100")
        self.label.pack()

    def update(self, subject: Subject) -> None:
        if isinstance(subject, GameCharacter):
            hp = subject.getHealth()
            max_hp = subject.getMaxHealth()
            percentage = (hp / max_hp) * 100
            self.progressBar['value'] = percentage
            self.label.config(text=f"HP: {hp}/{max_hp}")


class ManaBarUI(Observer):
    """Полоска маны"""
    def __init__(self, parent):
        self.frame = tk.Frame(parent)
        self.frame.pack(pady=5)
        
        tk.Label(self.frame, text="Мана:").pack()
        self.progressBar = ttk.Progressbar(self.frame, length=300, mode='determinate')
        self.progressBar.pack(pady=5)
        self.label = tk.Label(self.frame, text="MP: 50/50")
        self.label.pack()

    def update(self, subject: Subject) -> None:
        if isinstance(subject, GameCharacter):
            mp = subject.getMana()
            max_mp = subject.getMaxMana()
            percentage = (mp / max_mp) * 100
            self.progressBar['value'] = percentage
            self.label.config(text=f"MP: {mp}/{max_mp}")


class GoldCounterUI(Observer):
    """Счетчик золота"""
    def __init__(self, parent):
        self.label = tk.Label(parent, text="Gold: 0", font=('Arial', 14, 'bold'))
        self.label.pack(pady=10)

    def update(self, subject: Subject) -> None:
        if isinstance(subject, GameCharacter):
            gold = subject.getGold()
            self.label.config(text=f"Gold: {gold}")


class LevelUpNotification(Observer):
    """Уведомление о повышении уровня"""
    def __init__(self, parent):
        self.lastKnownLevel = 1
        self.notification_label = tk.Label(parent, text="", fg="green", font=('Arial', 12, 'bold'))
        self.notification_label.pack(pady=5)

    def update(self, subject: Subject) -> None:
        if isinstance(subject, GameCharacter):
            current_level = subject.getLevel()
            if current_level > self.lastKnownLevel:
                self.notification_label.config(text=f"🎉 LEVEL UP! Теперь уровень {current_level}!")
                messagebox.showinfo("Level Up!", f"Поздравляем! Вы достигли уровня {current_level}!")
                self.lastKnownLevel = current_level
            else:
                self.notification_label.config(text="")


class DeathNotification(Observer):
    """Уведомление о смерти персонажа"""
    def __init__(self, parent):
        self.gameOverPanel = None
        self.isGameOver = False
        self.death_label = tk.Label(parent, text="", fg="red", font=('Arial', 16, 'bold'))
        self.death_label.pack(pady=10)

    def update(self, subject: Subject) -> None:
        if isinstance(subject, GameCharacter) and not subject.isAlive() and not self.isGameOver:
            self.isGameOver = True
            self.death_label.config(text="💀 GAME OVER! Персонаж погиб!")
            messagebox.showerror("Game Over", "Ваш персонаж погиб! Игра окончена.")


class GameApp:
    """Главное приложение"""
    def __init__(self, root):
        self.root = root
        self.root.title("Game Dashboard - С паттерном Observer")
        self.root.geometry("500x600")
        
        # Заголовок
        tk.Label(root, text="Характеристики персонажа", 
                font=('Arial', 12, 'bold')).pack(pady=10)
        
        # Создание UI компонентов (Подписчиков)
        self.health_ui = HealthBarUI(root)
        self.mana_ui = ManaBarUI(root)
        self.gold_ui = GoldCounterUI(root)
        self.level_ui = LevelUpNotification(root)
        self.death_ui = DeathNotification(root)
        
        # Создание Издателя
        self.character = GameCharacter()
        
        # Подписка наблюдателей на персонажа
        print("\n=== ПОДПИСКА НАБЛЮДАТЕЛЕЙ ===")
        self.character.attach(self.health_ui)
        self.character.attach(self.mana_ui)
        self.character.attach(self.gold_ui)
        self.character.attach(self.level_ui)
        self.character.attach(self.death_ui)
        print(f"Всего подписчиков: {len(self.character._observers)}\n")
        
        # Инициализация UI
        self.character.notify()
        
        # Кнопки управления
        self._create_buttons()
        
        # Кнопка для демонстрации detach
        self._create_detach_button()
    
    def _create_buttons(self):
        """Создание кнопок"""
        control_frame = tk.Frame(self.root)
        control_frame.pack(pady=20)
        
        tk.Button(control_frame, text="💥 Получить урон (30)", 
                 command=lambda: self.character.takeDamage(30)).pack(fill='x', pady=2)
        
        tk.Button(control_frame, text="❤️ Лечение (20)", 
                 command=lambda: self.character.heal(20)).pack(fill='x', pady=2)
        
        tk.Button(control_frame, text="💰 Найти золото (50)", 
                 command=lambda: self.character.gainGold(50)).pack(fill='x', pady=2)
        
        tk.Button(control_frame, text="⚔️ Получить опыт (150)", 
                 command=lambda: self.character.gainXP(150)).pack(fill='x', pady=2)
        
        tk.Button(control_frame, text="🔮 Восстановить ману (20)", 
                 command=lambda: self.character.restoreMana(20)).pack(fill='x', pady=2)
        
        tk.Button(control_frame, text="✨ Произнести заклинание (15 маны)", 
                 command=lambda: self.character.castSpell(15)).pack(fill='x', pady=2)
    
    def _create_detach_button(self):
        """Кнопка для демонстрации отписки наблюдателя"""
        detach_frame = tk.Frame(self.root)
        detach_frame.pack(pady=10)
        
        self.detach_btn = tk.Button(detach_frame, text=" Отписать HealthBarUI", 
                                   command=self._toggle_health_ui)
        self.detach_btn.pack(fill='x')
    
    def _toggle_health_ui(self):
        """Переключение подписки HealthBarUI"""
        if self.health_ui in self.character._observers:
            self.character.detach(self.health_ui)
            self.detach_btn.config(text="✅ Подписать HealthBarUI")
        else:
            self.character.attach(self.health_ui)
            self.detach_btn.config(text="🚫 Отписать HealthBarUI")


if __name__ == "__main__":
    root = tk.Tk()
    app = GameApp(root)
    root.mainloop()