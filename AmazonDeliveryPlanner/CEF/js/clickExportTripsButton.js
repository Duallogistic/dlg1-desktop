(
    function ()
    {
        function getElementByXpath(path) {
            return document.evaluate(path, document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;
        }

        var xpath_b1 = "//*[@id='application']/div/div[3]/div[1]/div[2]/div[2]/button"; // trips button on tab 1
        var export_trips_button_1 = getElementByXpath(xpath_b1);
        console.log(export_trips_button_1);

        if (export_trips_button_1) {
            export_trips_button_1.click();
            return true;
        }

        var xpath_b2 = "//*[@id='application']/div/div[2]/div[2]/div[3]/button"; // trips button on tab 2
        var export_trips_button_2 = getElementByXpath(xpath_b2);
        console.log(export_trips_button_2);
        if (export_trips_button_2) {
            export_trips_button_2.click();
            return true;
        }

        var xpath_b3 = "//*[@id='application']/div/div[2]/div[1]/div[2]/div[2]/button"; // trips button on tab 3
        var export_trips_button_3 = getElementByXpath(xpath_b3);
        console.log(export_trips_button_3);
        if (export_trips_button_3) {
            export_trips_button_3.click();
            return true;
        }

        return false;        
    }
)();