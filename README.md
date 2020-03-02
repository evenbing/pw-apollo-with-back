<h2>PW Application</h2>
The application is for Parrot Wings (PW, “internal money”) transfer between system users.
<br/>
The application will be very “polite” and will inform a user of any problems (i.e. login not successful, not enough PW to remit the transaction, etc.)
<h3>User registration</h3>
Any person on Earth can register with the service for free, providing their Name (e.g. John Smith), valid email (e.g. jsmith@gmail.com) and password. 
<br/>
When a new user registers, the System will verify, that the user has provided a unique (not previously registered in the system) email, and also provided human name and a password. These 3 fields are mandatory. Password is to be typed twice for justification. No email verification required.
<br/>
On successful registration every User will be awarded with 500 (five hundred) PW starting balance.
<h3>Logging in</h3>
Users login to the system using their email and password.
<br/>
Users will be able to Log out.
<br/>
No password recovery, change password, etc. functions required.
<h3>PW</h3>
The system will allow users to perform the following operations:
<ol>
<li>See their Name and current PW balance always on screen</li>
<li>
Create a new transaction. To make a new transaction (PW payment) a user will
<ul>
<li>Choose the recipient by querying the  User list by name (autocomplete).</li>
<li>When a recipient chosen, entering the PW transaction amount. The system will check that the transaction amount is not greater than the current user balance.</li>
<li>Committing the transaction. Once transaction succeeded, the recipient account will be credited (PW++) by the entered amount of PW, and the payee account debited (PW--) for the same amount of PW. The system shall display PW balance changes immediately to the user.</li>
</ul>
</li>
<li>(Optional) Create a new transaction as a copy from a list of their existing transactions: create a handy UI for a user to browse their recent transactions, and select a transaction as a basis for a new transaction. Once old transaction selected, all its details (recipient, PW amount) will be copied to the new transaction.</li>
<li>
Review a list (history) of their transactions. A list of transactions will show the most recent transactions on top of the list and display the following info for each transaction:
<ul>
<li>Date/Time of the transaction</li>
<li>Correspondent Name</li>
<li>Transaction amount, (Debit/Credit  for PW transferred)</li>
<li>Resulting balance</li>
</ul>
</li>
</ol>
