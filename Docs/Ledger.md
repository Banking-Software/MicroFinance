## Ledgers are the backbone of the whole system. Every Transaction that happens is under one or more ledgers.

All of these Operations are authorized to admin only.

Ledger Section is divided into 4 sections  in the system.

## Account Type

There are four  fixed account types, and the basic need of account type is to group the ledger which then is used for analytics part.

Fix Account Types with its ID

> Id: 1 , Name: Assets

> Id: 2 , Namme: Expense

> Id:3 , Name: Income

> Id: 4 , Name: Liability

## Group Type

They are basically used to group the ledger. Every Group Type is under a certain Account Type. Main catch in the Group Type is that its ‘CharKhata Number’ is unique. So In the system Charkhata Number is used as a parameter to automate the transactional process. 

# Create GroupType

Create:

name*	string, minLength: 1

nepaliName	string, nullable: true

entryDate*	string($date)

schedule	integer($int32), nullable: true

accountTypeId*	integer($int32)

charKhataNumber*	string

# Update GroupType

id*	integer($int32)

name*	string, minLength: 1

schedule	integer($int32)

nepaliName	string, nullable: true


## Ledger

Every transaction happens under this Ledger in the system. When there is transfer, withdrawal or deposit, some ledgers are affected. Every Ledger is under some Group Type. ‘Id’ and 'LedgerCode' field is used to define the ledger uniquely in the system. This LedgerCode is represented as ‘GL Code’ in Frontend.

# Why Id and LedgerCode?

There are certain Ledger which should be exist beforehand. And these pre-existing ledger are later use to store transactional amount automatically. 

For ex: When someone deposit money, then 'Cash' Ledger is affected, so to automate this thing, we used LedgerCode (Fixed for these pre-existing ledger). System generally used this Ledger by first fetching the details using LedgerCode (Fix). 

Id is not used in this condition because, it is autogenerated and database can generate any Id (Not always sequencial)

And there is also a case when Sahakari User will add new Ledger, in this condition whatever the Id is assiged by database, we update same in LedgerCode also.

# Create Ledger

groupTypeId*	integer($int32)

name*	string, minLength: 1

nepaliName	string, nullable: true

entryDate*	string($date)

isSubLedgerActive*	boolean

depreciationRate	number($double), maximum: 100, minimum: 0, pattern: ^\d+(\.\d{1,2})?$, nullable: true

hisabNumber	string, nullable: true


# Update Ledger

id*	integer($int32)

nepaliName	string, nullable: true

name*	string, minLength: 1

isSubLedgerActive*	boolean

depreciationRate	number($double), maximum: 100, minimum: 0, pattern: ^\d+(\.\d{1,2})?$, nullable: true

hisabNumber	string, nullable: true

## SubLedger

Some Ledgers might have sub-part as well, so to handle this Subledger comes into picture. So when some transaction happens it might affect subledger rather than the whole ledger. There are fixed subledger so SubLedgerCode is used to define them uniquely.


# Create SubLedger

name*	string, minLength: 1

description	string, nullable: true

ledgerId*	integer($int32)


# Create SubLedger

id*	integer($int32)

name*	string, minLength: 1

description	string, nullable: true

# Update SubLedger

id*	integer($int32)

name*	string, minLength: 1

description	string, nullable: true

#########################################################################

## Bank setup

Every branches of Sahakari opens their account in a Bank, so their details comes here

While Creating Please note: 

1) bankBranch --> Name Bank's Branch where sahakari is opening an account

2) branchCode --> Branch Code of Sahakari Sanstha

So the idea is, each branch of sahakari can open separate bank account for them.

# Create

name*	string, minLength: 1

nepaliName	string, nullable: true

bankBranch*	string, minLength: 1

branchCode*	string, minLength: 1

accountNumber*	string, minLength: 1

bankTypeId*	integer($int32), pattern: ^[1-3], 

interestRate	number($double), maximum: 100, minimum: 0, pattern: ^\d+(\.\d{1,2})?$, nullable: true

# Update

id*	integer($int32)

nepaliName	string, nullable: true

bankBranch*	string, minLength: 1

bankTypeId*	integer($int32), pattern: ^[1-3]

interestRate*	number($double), maximum: 100, minimum: 0, pattern: ^\d+(\.\d{1,2})?$


# Bank Type

There are three bank type and they are fixed

ID: 1	, Name: Commercial

ID: 2	, Name: Development

ID: 3	, Name: Finance