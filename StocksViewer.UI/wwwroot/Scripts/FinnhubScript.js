﻿//Create a WebSocket to perform duplex (back-and-forth) communication with server
const token = document.querySelector("#FinnhubToken").value;
const socket = new WebSocket(`wss://ws.finnhub.io?token=${token}`);
var stockSymbol = document.getElementById("StockSymbol").value; //get symbol from input hidden
console.log("I am in the script!");

// Connection opened. Subscribe to a symbol
socket.addEventListener('open', function (event) {
    socket.send(JSON.stringify({ 'type': 'subscribe', 'symbol': stockSymbol }))
});

// Listen (ready to receive) for messages
socket.addEventListener('message', function (event) {

    //if error message is received from server
    if (event.data.type == "error") {
        $("#current-price").text(event.data.msg);
        return; //exit the function
    }

    //data received from server
    //console.log('Message from server ', event.data);

    /* Sample response:
    {"data":[{"p":220.89,"s":"MSFT","t":1575526691134,"v":100}],"type":"trade"}
    type: message type
    data: [ list of trades ]
    s: symbol of the company
    p: Last price
    t: UNIX milliseconds timestamp
    v: volume (number of orders)
    c: trade conditions (if any)
    */

    var eventData = JSON.parse(event.data);
    if (eventData) {
        if (eventData.data) {
            //get the updated price
            var updatedPrice = JSON.parse(event.data).data[0].p;
            //console.log(JSON.parse(event.data).data[0].t);
            var date = new Date(JSON.parse(event.data).data[0].t);
            //console.log(date);
            //console.log(updatedPrice);

            console.log("I got a new price!")

            //update the UI
            $("#current-price").text(updatedPrice.toFixed(2)); //price - big display
            $("#current-time").text(date.toLocaleTimeString("en-US")); //time - big display
        }
    }
});

// Unsubscribe
var unsubscribe = function (symbol) {
    //disconnect from server
    socket.send(JSON.stringify({ 'type': 'unsubscribe', 'symbol': symbol }))
}

//when the page is being closed, unsubscribe from the WebSocket
window.onunload = function () {
    unsubscribe(stockSsymbol);
};
