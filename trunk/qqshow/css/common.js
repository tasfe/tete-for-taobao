function createxmlHttpRequest()
        {
            if(window.ActiveXObject)
                xmlHttp = new ActiveXObject("Microsoft.XMLHTTP");
            else if(window.XMLHttpRequest)
                xmlHttp = new XMLHttpRequest();
        }
        function createQueryString()
        {
            var firstName = document.getElementById("firstName").value;
            var birthday = document.getElementById("birthday").value;
            var queryString = "firstName="+firstName+"&birthday="+birthday;
            return encodeURI(encodeURI(queryString));//防止乱码
        }
        function spreadStat(idea, itemid)
        {
            createxmlHttpRequest();
            var queryString = "http://www.7fshop.com/show/plist.aspx?act=hit&idea="+idea+"&itemid="+itemid+"&url="+escape(url)+"&t="+new Date().getTime();
            xmlHttp.open("GET",queryString);
            xmlHttp.send(null);
        }
        function doRequestUsingPost()
        {
            createxmlHttpRequest();
            var url ="AjaxHandler.ashx?timestamp="+new Date().getTime();
            var queryString = createQueryString();
            xmlHttp.open("POST",url);
            xmlHttp.onreadystatechange = handleStateChange;
            xmlHttp.setRequestHeader("Content-Type","application/x-www-form-urlencoded");
            xmlHttp.send(queryString);
        }
        function handleStateChange()
        {
            if(xmlHttp.readyState==4 && xmlHttp.status == 200)
            {
                //var responseDiv = document.getElementById("serverResponse");
                //responseDiv.innerHTML=decodeURI(xmlHttp.responseText);
            }
        }