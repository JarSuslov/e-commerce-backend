# ECommerce API

REST API для интернет-магазина на ASP.NET Core + PostgreSQL.

## Стек технологий

- .NET 10 / ASP.NET Core Web API
- PostgreSQL 16 + Entity Framework Core (Code First)
- JWT-авторизация (access token)
- FluentValidation
- Swagger (Swashbuckle)
- Docker + docker-compose
- Rate Limiting (Microsoft.AspNetCore.RateLimiting)

## Быстрый запуск (Docker)

```bash
docker-compose up --build
```

API будет доступен по адресу: **http://localhost:8080**  
Swagger UI: **http://localhost:8080**

## Тестовые учётные записи (Seed)

| Роль     | Email              | Пароль        |
|----------|--------------------|---------------|
| Admin    | admin@store.com    | Admin123!     |
| Customer | customer@store.com | Customer123!  |

## Как тестировать в Swagger

1. Откройте http://localhost:8080 в браузере
2. Выполните `POST /api/auth/login` с телом:
   ```json
   {
     "email": "admin@store.com",
     "password": "Admin123!"
   }
   ```
3. Скопируйте значение `token` из ответа
4. Нажмите кнопку **Authorize** (🔒) в верхней части страницы
5. Введите токен (без префикса `Bearer`)
6. Нажмите **Authorize** — теперь все защищённые эндпоинты доступны

## Эндпоинты

### Аутентификация
| Метод | URL                  | Описание                |
|-------|----------------------|-------------------------|
| POST  | /api/auth/register   | Регистрация покупателя  |
| POST  | /api/auth/login      | Авторизация (получение JWT) |

### Категории (Admin для CUD, GET — публичный)
| Метод  | URL                    | Описание             |
|--------|------------------------|----------------------|
| GET    | /api/categories        | Список категорий     |
| GET    | /api/categories/{id}   | Категория по ID      |
| POST   | /api/categories        | Создать категорию    |
| PUT    | /api/categories/{id}   | Обновить категорию   |
| DELETE | /api/categories/{id}   | Удалить категорию    |

### Товары (Admin для CUD, GET — публичный)
| Метод  | URL                  | Описание                          |
|--------|----------------------|-----------------------------------|
| GET    | /api/products        | Список с фильтрами + пагинация   |
| GET    | /api/products/{id}   | Товар по ID                       |
| POST   | /api/products        | Создать товар                     |
| PUT    | /api/products/{id}   | Обновить товар                    |
| DELETE | /api/products/{id}   | Удалить товар                     |

Query-параметры для GET /api/products:
- `search` — поиск по названию
- `categoryId` — фильтр по категории
- `page` — номер страницы (по умолчанию 1)
- `pageSize` — размер страницы (по умолчанию 10, максимум 50)

### Корзина (требуется авторизация)
| Метод  | URL                        | Описание                  |
|--------|----------------------------|---------------------------|
| GET    | /api/cart                  | Просмотр корзины          |
| POST   | /api/cart                  | Добавить товар в корзину  |
| DELETE | /api/cart/{productId}      | Удалить товар из корзины  |

### Заказы (требуется авторизация)
| Метод | URL                  | Описание                    |
|-------|----------------------|-----------------------------|
| POST  | /api/orders          | Оформить заказ (checkout)   |
| GET   | /api/orders          | Мои заказы                  |
| GET   | /api/orders/{id}     | Заказ по ID                 |

### Админ — все заказы
| Метод | URL                  | Описание                    |
|-------|----------------------|-----------------------------|
| GET   | /api/admin/orders    | Все заказы (только Admin)   |

## Локальная разработка (без Docker)

1. Установите PostgreSQL и создайте БД `ecommerce_db`
2. Обновите строку подключения в `appsettings.Development.json`
3. Запустите:
```bash
cd ECommerce.API
dotnet ef migrations add InitialCreate
dotnet run
```

## Структура проекта

```
ECommerce.API/
├── Controllers/          # API-контроллеры
├── Common/
│   ├── Exceptions/       # Кастомные исключения
│   ├── Middleware/        # ExceptionHandlingMiddleware
│   └── Extensions/       # Extension-методы
├── Data/                 # DbContext + Seed
├── DTOs/                 # Request/Response модели
│   ├── Auth/
│   ├── Cart/
│   ├── Category/
│   ├── Order/
│   └── Product/
├── Entities/             # EF Core сущности
├── Services/             # Бизнес-логика
│   └── Interfaces/
├── Validators/           # FluentValidation
├── Program.cs            # Точка входа + конфигурация
├── appsettings.json
└── appsettings.Development.json
```
