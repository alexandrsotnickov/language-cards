# Language Cards API

Language Cards API — backend REST API для приложения изучения языков на основе карточек.  
Сервис отвечает за управление карточками, пользователями и учебным прогрессом.

## Особенности

- REST API для языковых карточек
- Поддержка многопользовательского режима
- Подготовка к аутентификации и авторизации
- Управление темами пользователей, подписками на них, карточками в темах

## Стек

- ASP.NET Core
- C#
- REST API
- Entity Framework Core
- PostgreSQL

## Требования

-Windows 10 Pro/Enterprise/Home 22H2 (19045) и выше
-Ubuntu 22.04 LTS и выше

## Развертывание (для Ubuntu)

Установка Docker
```bash
sudo apt install -y docker.io docker-compose
```
Добавление пользователя в docker-группу

```bash
sudo usermod -aG docker $USER
```
Проверка работоспособности docker

```bash
docker --version
```

## Run
```bash
dotnet run
```
По умолчанию API доступен по адресу:

- https://localhost:7192
- http://localhost:5192







