# UMP Project

## ����
��� ����� � ��������������� ��������� ���-API �� ���������� ��������� ���������� (CLI), ����������� �� ������������� .NET. ����� ��������� ������ ����������� ����� JWT, ������� � ����� ����� PostgreSQL ����� Entity Framework Core �� ������ ����������� �볺��-������.

### ������ �����
- **���� �� �������㳿**:
  - C# (.NET 8)
  - Entity Framework Core ��� ������ � PostgreSQL
  - JWT ��� �����������
  - System.CommandLine ��� CLI
  - Swashbuckle.AspNetCore ��� Swagger ������������

- **��������� ������**:
  - `UMP-API`: ���-API � ���������� ��� ����������� (`/api/auth/login`) �� ������������ (`/api/market`).
  - `UMP-CLI`: ��������� ����� ��� �����䳿 � API (������� `login` � `market list`).

- **������������**:
  - ���� �����: PostgreSQL (�������� ��� ����� Docker).
  - ������������: `appsettings.json` �� ConnectionString �� JWT-�����������.
  - ����� ���������� � `~/.ump/config.json` ���� �����.

- **�� ���������**:
  1. �����������, �� .NET 8 SDK �����������.
  2. �������� PostgreSQL � ����� `ConnectionString` � `UMP-API/appsettings.json`.
  3. � ����� `UMP-API` �������:
     ```
      dotnet restore
      dotnet ef database update
      dotnet run
	 ```
  1. 4. � ����� `UMP-CLI` �������:
       ``` 
      dotnet run -- login --username testuser --password testpass
      dotnet run -- market list
     ```
   - **������� ��� ����������**:

��� �������� ��� �������� ������� � ������������.
����� �� ���������� �� ���������.
  
   - **���� ��������**:

������ �������� ��� CRUD-�������� �� �������������.
����������� ������� ������� � ���������.
��������� CLI ������ ���������.

   - **�����������**:
������� ������� �� ��������! ³����� ������ ������� ��� �����������.

