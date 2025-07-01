# SpamFilterDemo

![.NET](https://img.shields.io/badge/.NET-9.0-informational)
![License](https://img.shields.io/badge/license-MIT-green)
![Build](https://img.shields.io/badge/build-passing-brightgreen)

Демонстрационное WinForms‑приложение для фильтрации спама.  
Под капотом два классических алгоритма:

1. **Наивный байесовский классификатор** (с сглаживанием Лапласа)  
2. **Метод Фишера** (подход П. Грэма / SpamBayes)

> Цель проекта — показать, насколько просто реализовать статистический фильтр
> спама на чистом C# без сторонних ML‑библиотек.

---

## ✨ Возможности
- Загрузка корпуса (`spam/`, `ham/`) любой размерности  
- Параллельное обучение **обоих** алгоритмов одним кликом  
- Классификация текста в реальном времени  
- Адаптивный UI — элементы растягиваются при изменении размера окна  
- Таймер в заголовке во время обучения («`Обучение: 3,7 с`»)  
- Красивые сообщения об ошибках вместо падений

---

## 🚀 Быстрый старт
```bash
git clone https://github.com/Avazbek22/SpamFilterDemo.git
cd SpamFilterDemo
dotnet build -c Release
```
1. Запустите `bin/Release/net8.0-windows/SpamFilterDemo.exe`.  
2. Выберите папку с двумя подпапками **`spam`** и **`ham`**.  
3. Нажмите **«Обучить»** и дождитесь сообщения «Готово».  
4. Введите (или вставьте) текст письма → **«Классифицировать»**.

> 👉 Не хотите собирать корпус? Скачайте готовый
> [ru_spam_dataset_big.zip](https://github.com/Avazbek22/SpamFilterDemo/releases) (20 000 писем).

---

## 🗂️ Структура проекта
```
SpamFilterDemo/
│  SpamFilterDemo.csproj
│  Program.cs
├─ Core/
│    Tokenizer.cs
│    TrainingSet.cs
│    ISpamClassifier.cs
│    NaiveBayesClassifier.cs
│    FisherClassifier.cs
└─ UI/
     MainForm.cs
     MainForm.Designer.cs
```

---

## 📖 Алгоритмы

| Алгоритм | Формула | Особенности |
|----------|---------|-------------|
| **Naive Bayes** | ![eq](https://latex.codecogs.com/svg.image?P(C%20%7C%20w_1%2C%5Cdots%2Cw_k)%20%3D%20%5Cfrac%7BP(C)%20%5Cprod_i%20P(w_i%7C%20C)%7D%7B%5Csum_%7BC'%7D%20P(C')%20%5Cprod_i%20P(w_i%7C%20C')%7D) | лог‑вероятности, сглаживание α = 1 |
| **Fisher** | ![eq](https://latex.codecogs.com/svg.image?%5Cchi%5E2%3D-2%20%5Csum_i%20%5Cln%20p_i) | χ²‑CDF даёт P‑value; порог 0,5 |

---

## 🧪 Тесты и метрики

| Корпус | Accuracy | Precision<sub>spam</sub> | Recall<sub>spam</sub> |
|--------|----------|--------------------------|-----------------------|
| 20 000 писем | 0.982 (Bayes) / 0.967 (Fisher) | 0.975 / **0.989** | **0.989** / 0.945 |

Профилирование Ryzen 7600X: обучение 10 000 + 10 000 писем ≈ 9 с.

---

## ⚙️ Стек технологий
- **C# 13** / **.NET 9** (Windows Desktop)
- WinForms
- Task‑based параллелизм
- PlantUML (диаграммы классов)
- MSTest (модульные тесты)

---

## 🛣️ Дорожная карта
- [ ] Сериализация обученной модели в JSON/ONNX  
- [ ] Добавить TF‑IDF и биграммы  
- [ ] Простой REST‑API (ASP.NET Minimal API)  
- [ ] CI‑workflow (GitHub Actions, Windows VM)

---

## 📜 Лицензия
Код распространяется под лицензией **MIT** — см. файл *LICENSE*.

---

> **Made with ♥ by Avazbek**  
> *Have fun fighting spam!* 🚀
