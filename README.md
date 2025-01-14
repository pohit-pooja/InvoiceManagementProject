# Invoice Management Project

## Overview
The **Invoice Management Project** is an application designed to efficiently manage invoices, including their creation, retrieval, payment processing, and overdue handling. It employs a layered architecture with an MVC pattern, ensuring separation of concerns, scalability, and maintainability.

---

## Features

### **Invoice Status Defaults**
- All new invoices are created with a default status of `Pending`.

### **Date Validation**
- The `DueDate` must not be in the past when creating an invoice.
- All system dates are calculated in UTC.

### **Amount Validation**
- The `Amount` field must be a positive decimal value greater than 0.
- Payment amounts must not exceed the current invoice balance.

### **Overdue Processing**
- Invoices overdue by a defined number of days are updated with a status of:
  - `Paid` if partially paid.
  - `Void` if no payment has been made.
- Late fees are applied when creating new invoices for overdue amounts.

### **Payment Processing**
- Supports **partial payments**, updating the `PaidAmount` and `Amount` fields accordingly.
- Automatically marks invoices as `Paid` if the balance is reduced to zero.

### **Error Handling**
- Exceptions are thrown for invalid input, such as:
  - Negative overdue days.
  - Negative late fees.
  - Invalid payment amounts.
- Proper validation messages are included for all validation failures.

---

## Added Functionality

### **Overdue Invoice Handling**
- Overdue invoices can be processed in bulk, with support for:
  - Configurable overdue days.
  - Late fees.
- Automatically creates a new invoice for any remaining overdue amount, applying the late fee.

### **Integration with Repository**
- All database interactions are handled through a **Repository Pattern**:
  - Ensures abstraction and testability.

### **Data Validation**
- Ensures invalid data (e.g., past due dates, negative amounts) is rejected.
- Conditionally processes invoices based on business rules, including status updates for fully paid invoices.

---

## Assumptions
1. **Invoice Status**: New invoices default to `Pending`.
2. **Date Validation**: `DueDate` cannot be in the past.
3. **Amount Validation**:
   - `Amount` must be a positive decimal greater than 0.
   - Payment cannot exceed the current balance.
4. **System Dates**: All dates are managed in UTC.

---

### Setup
1. Clone the repository:
   ```bash
   git clone <repository-url>
