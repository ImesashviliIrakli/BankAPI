# BankAPI Project

BankAPI is a .NET 8 Web API following Clean Architecture principles. It offers various endpoints for user registration, authentication, account management, and financial transactions.

## API Endpoints
![image](https://github.com/ImesashviliIrakli/BankAPI/assets/77686006/84791f42-b87e-454e-a5cc-031db4c3f726)

### 1. Register
- **Endpoint:** `/api/auth/register`
- **Description:** Registers a new user, creating a wallet with balances in three currencies (GEL, USD, EUR).

### 2. Login and Get Token
- **Endpoint:** `/api/auth/login`
- **Description:** Authenticates the user and provides a JWT token for authorization.

### 3. Authorization
- **Usage:** Before testing other endpoints, use the provided JWT token by clicking the "Authorize" button in the top right corner and entering: `Bearer [YOUR TOKEN]`.

### 4. Get Accounts
- **Endpoint:** `/api/wallet/getaccounts`
- **Description:** Retrieves account details for the logged-in user.
- **Endpoint:** `/api/wallet/getaccounts/{currency}`
- **Description:** Retrieves account details for the logged-in user and filters it by currency.

### 5. Deposit
- **Endpoint:** `/api/transactions/deposit`
- **Description:** Allows the user to deposit funds into a specific account by providing the account number in the request body.

### 6. Withdraw
- **Endpoint:** `/api/transactions/withdraw`
- **Description:** Enables users to withdraw funds from a specific account using the account number in the request body.

### 7. Create New Wallet (For Logged In User)
- **Endpoint:** `/api/wallet/createwalletnew`
- **Description:** Creates a new wallet for the logged-in user.

### 8. Transfer
- **Endpoint:** `/api/transactions/transfer`
- **Description:** Facilitates fund transfers between accounts with detailed error handling.

### Note on Total Balance
The project plans to include total balance functionality in the future, considering currency rates.

## Local Testing

To test the project locally:

1. **Clone the repository.**
2. **Verify the Database Configuration:** Check the `DefaultConnection` to ensure it points to the correct server.
3. **Apply Database Migrations:** Run `Update-Database` with the **Infrastructure** project selected.

Explore the endpoints locally, following the described steps for registration, authentication, and various financial transactions.

