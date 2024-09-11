
# MessageService API
This project was created by using ASP.NET Core MVC, PostgreSQL, xUnit, Moq, WebSocket, Swagger. 
## Info

To deploy in a docker container:
```
cd ./Docker
docker-compose up -d
```
Base URL: http://localhost:5000  
Swagger URL: http://localhost:5000/swagger



## Paths

### /api/History

- **GET**: Получает сообщения за указанный период времени.
  - **Tags**: History
  - **Parameters**:
    - **startDate**: Начальная дата периода. (query, string, date-time)
    - **endDate**: Конечная дата периода. (query, string, date-time)
  - **Responses**:
    - **200**: OK. Возвращает список сообщений.
      - **Content**:
        - **text/plain**: Array of [Message](#componentsschemasmessage)
        - **application/json**: Array of [Message](#componentsschemasmessage)
        - **text/json**: Array of [Message](#componentsschemasmessage)

### /Pages/handler

- **GET**: Возвращает страницу перехватчика сообщений.
  - **Tags**: Pages
  - **Responses**:
    - **200**: OK. Возвращает страницу перехватчика сообщений.
      - **Content**:
        - **text/plain**: [ViewResult](#componentsschemasviewresult)
        - **application/json**: [ViewResult](#componentsschemasviewresult)
        - **text/json**: [ViewResult](#componentsschemasviewresult)

### /Pages/sender

- **GET**: Возвращает страницу отправителя сообщений.
  - **Tags**: Pages
  - **Responses**:
    - **200**: OK. Возвращает страницу отправителя сообщений.
      - **Content**:
        - **text/plain**: [ViewResult](#componentsschemasviewresult)
        - **application/json**: [ViewResult](#componentsschemasviewresult)
        - **text/json**: [ViewResult](#componentsschemasviewresult)

### /Pages/history

- **GET**: Возвращает страницу истории.
  - **Tags**: Pages
  - **Responses**:
    - **200**: OK. Возвращает страницу истории сообщений.
      - **Content**:
        - **text/plain**: [ViewResult](#componentsschemasviewresult)
        - **application/json**: [ViewResult](#componentsschemasviewresult)
        - **text/json**: [ViewResult](#componentsschemasviewresult)

### /api/Sender

- **POST**: Отправляет новое сообщение.
  - **Tags**: Sender
  - **Request Body**: Сообщение для отправки.
    - **Content**:
      - **application/json**: [Message](#componentsschemasmessage)
      - **text/json**: [Message](#componentsschemasmessage)
      - **application/*+json**: [Message](#componentsschemasmessage)
  - **Responses**:
    - **200**: OK. Сообщение успешно отправлено.

## Components

#### Message

- **Type**: Object
- **Properties**:
  - **id**: integer (int32)
  - **text**: string (nullable)
  - **timestamp**: string (date-time)
  - **sequenceNumber**: integer (int32)
- **Additional Properties**: false
