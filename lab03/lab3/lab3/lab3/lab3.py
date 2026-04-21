import tkinter as tk
from tkinter import ttk, messagebox


class HealthBarUI:
    """Полоска здоровья"""
    def __init__(self, parent):
        self.frame = tk.Frame(parent)
        self.frame.pack(pady=5)
        tk.Label(self.frame, text="Здоровье:").pack()
        self.progressBar = ttk.Progressbar(self.frame, length=300, mode='determinate')
        self.progressBar.pack(pady=5)
        self.label = tk.Label(self.frame, text="HP: 100/100")
        self.label.pack()

    def update_health(self, current_health, max_health):
        percentage = (current_health / max_health) * 100
        self.progressBar['value'] = percentage
        self.label.config(text=f"HP: {current_health}/{max_health}")


class ManaBarUI:
    """Полоска маны"""
    def __init__(self, parent):
        self.frame = tk.Frame(parent)
        self.frame.pack(pady=5)
        tk.Label(self.frame, text="Мана:").pack()
        self.progressBar = ttk.Progressbar(self.frame, length=300, mode='determinate')
        self.progressBar.pack(pady=5)
        self.label = tk.Label(self.frame, text="MP: 50/50")
        self.label.pack()

    def update_mana(self, current_mana, max_mana):
        percentage = (current_mana / max_mana) * 100
        self.progressBar['value'] = percentage
        self.label.config(text=f"MP: {current_mana}/{max_mana}")


class GoldCounterUI:
    """Счетчик золота"""
    def __init__(self, parent):
        self.label = tk.Label(parent, text="Gold: 0", font=('Arial', 14, 'bold'))
        self.label.pack(pady=10)

    def update_gold(self, gold):
        self.label.config(text=f"Gold: {gold}")


class LevelUpNotification:
    """Уведомление о повышении уровня"""
    def __init__(self, parent):
        self.lastKnownLevel = 1
        self.notification_label = tk.Label(parent, text="", fg="green", font=('Arial', 12, 'bold'))
        self.notification_label.pack(pady=5)

    def check_level_up(self, current_level):
        if current_level > self.lastKnownLevel:
            self.notification_label.config(text=f"🎉 LEVEL UP! Теперь уровень {current_level}!")
            messagebox.showinfo("Level Up!", f"Поздравляем! Вы достигли уровня {current_level}!")
            self.lastKnownLevel = current_level
        else:
            self.notification_label.config(text="")


class DeathNotification:
    """Уведомление о смерти персонажа"""
    def __init__(self, parent):
        self.gameOverPanel = None
        self.isGameOver = False
        self.death_label = tk.Label(parent, text="", fg="red", font=('Arial', 16, 'bold'))
        self.death_label.pack(pady=10)

    def check_death(self, current_health):
        if current_health <= 0 and not self.isGameOver:
            self.isGameOver = True
            self.death_label.config(text="💀 GAME OVER! Персонаж погиб!")
            messagebox.showerror("Game Over", "Ваш персонаж погиб! Игра окончена.")


class GameCharacter:
   
    def __init__(self, health_bar, mana_bar, gold_counter, level_notification, death_notification):
        self.health = 100
        self.maxHealth = 100
        self.mana = 50
        self.maxMana = 50
        self.gold = 0
        self.experience = 0
        self.level = 1
        self.isAlive = True

        # ЖЕСТКИЕ ССЫЛКИ на конкретные объекты UI
        self.health_bar = health_bar
        self.mana_bar = mana_bar
        self.gold_counter = gold_counter
        self.level_notification = level_notification
        self.death_notification = death_notification

        self.is_health_bar_connected = True

        self._update_all_ui()

    def toggle_health_bar_connection(self):
        
        self.is_health_bar_connected = not self.is_health_bar_connected
        state = "ПОДКЛЮЧЕНА" if self.is_health_bar_connected else "ОТКЛЮЧЕНА"
        print(f"[DEBUG] HealthBar {state}")

    def _update_all_ui(self):
        if self.is_health_bar_connected:
            self.health_bar.update_health(self.health, self.maxHealth)
        self.mana_bar.update_mana(self.mana, self.maxMana)
        self.gold_counter.update_gold(self.gold)
        self.level_notification.check_level_up(self.level)

    def take_damage(self, amount):
        if not self.isAlive: return
        self.health = max(0, self.health - amount)
        if self.health == 0: self.isAlive = False

        if self.is_health_bar_connected:
            self.health_bar.update_health(self.health, self.maxHealth)
        
        self.death_notification.check_death(self.health)

    def heal(self, amount):
        if not self.isAlive: return
        self.health = min(self.maxHealth, self.health + amount)
        
        if self.is_health_bar_connected:
            self.health_bar.update_health(self.health, self.maxHealth)

    def gain_gold(self, amount):
        if not self.isAlive: return
        self.gold += amount
        self.gold_counter.update_gold(self.gold)

    def gain_xp(self, amount):
        if not self.isAlive: return
        self.experience += amount
        while self.experience >= self.level * 100:
            self.experience -= self.level * 100
            self.level += 1
            self.level_notification.check_level_up(self.level)

    def restore_mana(self, amount):
        if not self.isAlive: return
        self.mana = min(self.maxMana, self.mana + amount)
        self.mana_bar.update_mana(self.mana, self.maxMana)

    def cast_spell(self, cost):
        if not self.isAlive: return False
        if self.mana >= cost:
            self.mana -= cost
            self.mana_bar.update_mana(self.mana, self.maxMana)
            return True
        return False


class GameApp:
    """Главное приложение"""
    def __init__(self, root):
        self.root = root
        self.root.title("Game Dashboard - БЕЗ паттерна Observer")
        self.root.geometry("500x650")

        tk.Label(root, text="Характеристики персонажа", font=('Arial', 12, 'bold')).pack(pady=10)

        health_frame = tk.Frame(root)
        health_frame.pack(pady=5)
        self.health_bar = HealthBarUI(health_frame)

        mana_frame = tk.Frame(root)
        mana_frame.pack(pady=5)
        self.mana_bar = ManaBarUI(mana_frame)

        self.gold_counter = GoldCounterUI(root)
        self.level_notification = LevelUpNotification(root)
        self.death_notification = DeathNotification(root)

        self.character = GameCharacter(
            self.health_bar, self.mana_bar, self.gold_counter, 
            self.level_notification, self.death_notification
        )

        self._create_buttons()

    def _create_buttons(self):
        control_frame = tk.Frame(self.root)
        control_frame.pack(pady=20)

        tk.Button(control_frame, text="💥 Получить урон (30)", command=lambda: self.character.take_damage(30)).pack(fill='x', pady=2)
        tk.Button(control_frame, text="❤️ Лечение (20)", command=lambda: self.character.heal(20)).pack(fill='x', pady=2)
        tk.Button(control_frame, text="💰 Найти золото (50)", command=lambda: self.character.gain_gold(50)).pack(fill='x', pady=2)
        tk.Button(control_frame, text="⚔️ Получить опыт (150)", command=lambda: self.character.gain_xp(150)).pack(fill='x', pady=2)
        tk.Button(control_frame, text="🔮 Восстановить ману (20)", command=lambda: self.character.restore_mana(20)).pack(fill='x', pady=2)
        tk.Button(control_frame, text="✨ Заклинание (15 маны)", command=lambda: self.character.cast_spell(15)).pack(fill='x', pady=2)

        # Кнопка для демонстрации "костыльной" отписки
        self.toggle_btn = tk.Button(control_frame, text="🔌 Отписать HealthBar ", 
                                    command=self._toggle_health_connection)
        self.toggle_btn.pack(fill='x', pady=10)

    def _toggle_health_connection(self):
        self.character.toggle_health_bar_connection()
        current_state = self.character.is_health_bar_connected
        
        if current_state:
            self.toggle_btn.config(text="🔌 Отписать HealthBar")
        else:
            self.toggle_btn.config(text="🔌 Подписать HealthBar")


if __name__ == "__main__":
    root = tk.Tk()
    app = GameApp(root)
    root.mainloop()