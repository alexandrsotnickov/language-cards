# Language Cards API

Language Cards API — backend REST API для приложения изучения языков на основе карточек.  
Сервис отвечает за управление карточками, пользователями и учебным прогрессом.

## Особенности

- REST API для языковых карточек
- Поддержка многопользовательского режима
- Подготовка к аутентификации и авторизации
- Управление темами пользователей, подписками на них, карточками в темах

## Стек

- C#
- .NET 8
- ASP.NET Core
- REST API
- Entity Framework Core
- PostgreSQL

## Требования

- Windows 10 Pro/Enterprise/Home 22H2 (19045) и выше
- Ubuntu 22.04 LTS и выше

## Развертывание (для Ubuntu)

Установите pip3 для python (docker-compose без этого может не работать) 

```bash
sudo apt install python3-pip
pip3 --version
```

Установка Docker

```bash
sudo apt install -y docker.io docker-compose
```
Добавление пользователя в docker-группу (после этого перезагрузить ПК/VM)

```bash
sudo usermod -aG docker $USER
```
Проверка работоспособности docker и docker-compose

```bash
docker --version
docker-compose -v
```
Клонировать репозиторий

```bash
cd ~
git clone https://github.com/<REPO>
cd REPO
```
## Run
```bash
dotnet run
```
По умолчанию API доступен по адресу:

- https://localhost:7192
- http://localhost:5192







