Prime Time - WinForms

Структура рассчитана на существующие проекты:
- App_Model_Essence
- App_Model_TestLogics

1. Положите папку App_WinForms рядом с App_Model_Essence и App_Model_TestLogics.
2. Добавьте App_WinForms.csproj в существующее решение.
3. Проверьте TargetFramework в App_WinForms.csproj. Сейчас стоит net9.0-windows. Он должен совпадать с целевым фреймворком ваших существующих проектов.
4. Запустите App_WinForms.

WinForms использует ILogics и не содержит бизнес-логики. Бизнес-операции остаются в App_Model_TestLogics.

Формы:
- MainForm - выбор роли
- EmployeeForm - CRUD тренеров/атлетов и закрепление атлетов
- UserForm - добавление атлета, регистрация, персональная тренировка, подбор тренера и рейтинг
