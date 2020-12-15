# ItemStockChecker

This is a program that is used to check the stock of products for various websites. After the product is found to be in stock, you will get a text with a link to the product page so you can quickly make your purchase. This is a very early version of this. I hope to add lots of useful features and encourage sugestions/pull requests.

This is a simple .NET 5 executable. This can be built for Windows or Linux.

Before running the application, be sure to update the app.config file with the appropriate values.

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
