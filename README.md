# ItemStockChecker

This is a program that is used to check the stock of products for various websites. After the product is found to be in stock, you will get a text with a link to the product page so you can quickly make your purchase. This is a very early version of this. I hope to add lots of useful features and encourage sugestions/pull requests.

This is a simple .NET 5 executable. This can be built for Windows or Linux.

Before running the application, be sure to update the ItemStockChecker.dll.config file with the appropriate values.

# Config settings that need to be set before the first run

* Inside <urls> you must put your urls for the pages there. There are some inside the current config as an example. Note: you must escape xml special characters when pasting in urls. Otherwise you will get an exception when you try to start the application
  
* PhoneNumber needs to set just as a number (no dashes or parentheses).
* Each phone provider section has a enabled that you can use to turn it on or off [true|false]

* YourEmail: This must be a gmail.
* AppPassword: Please visit https://support.google.com/accounts/answer/185833?p=InvalidSecondFactor&visit_id=637421952488948393-2531523670&rd=1 to see how to generate this value

* PageRefreshTime: This is how often a page will be checked (ms)
* CoolDownTime: This is how long the program will wait after being temporarily banned (min). This value will double each time the cool down hits 0 and still has an error.

# Supported websites

* Amazon
* Best Buy
* B&H
* Micro Center
* New Egg

# Supported Phone providers:

* Verizon
* T-Mobile

* Must use gmail account

# Gatchyas

* Bestbuy links get blocked by t-mobile (idk why, it just doesnt go through their spam protection)

# Other stuff

* If you are temporarily blocked by a website, it will wait some time before it reattempts to check the site. If you are still blocked after the cool down, it will wait for double the time. This will continue until you can connect again.

* Add urls in the app.config UrlList/urls section. Note: These urls need to be escaped otherwise they will be invalid when loading them in.

Each url is run on its own thread, letting multiple urls be checked asynchronously.

Example output:
![example_output](https://i.imgur.com/GM0PLmn.png)

![example_output_gif](https://media.giphy.com/media/GGBUgdqJPYKCzJftoV/giphy.gif)

# TODOs:

* Add more phone providers
* Add more websites
* Add email support (with config toggle)
* Various bug fixes
