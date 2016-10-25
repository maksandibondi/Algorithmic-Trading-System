# Algorithmic-Trading-System

1. Algorithm Project is a library with class implementation of different algorithms which logic is stored in
user-defined libraries

2. FinamLib is a WebCrawler library

3. DBconnectorLib is a library storing quering logic for basic Data Access functions (CRUD, Insertion, Selection, Backup etc.)

4. Sender is a library with class implementation of ISender and IListener interfaces for ActiveMQ broker interconnection

5. TradingSystem is a library with class implementation of the observer pattern (publisher and subscriber classes).
Publisher - is an Algorithm user to work with the Data. 
Subscriber is an event handler to send transactions to ActiveMQ broker using the Sender.

6. TradingSystemExe is the executable that will execute Publisher, Subscriber, Crawler, Connector on separate threads

7. Databases is a folder where the sql backups of databases and actual Crawled Data to be written into DBs are stored
