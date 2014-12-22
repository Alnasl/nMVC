var RESTClient = function()
{
    this.RESTHandlerRequestKey = "NMVC_API_V1_REQUEST"
    this.RESTHandlerRequestParameter = "";
    this.AdditionalParameters = {};
    this.FormParameters = {};
    this.RequestType = "GET";
    this.RequestPath = "/";
    
    this.SuccessHandler = function(result, status, xhr) 
    {        
        Logger.Log(this.RESTHandlerRequestKey 
            + ": " 
            +  this.RESTHandlerRequestParameter 
            + " Unhandled AJAX success");        
    },
    
    this.ErrorHandler = function(xhr, status, err) 
    {        
        Logger.Log(this.RESTHandlerRequestKey 
            + ": " 
            +  this.RESTHandlerRequestParameter 
            + "Unhandled AJAX error: " 
            + JSON.stringify(err, null, 2));        
    }
    
    this.AddParameter = function(key, value)
    {
        this.AdditionalParameters[key] = value;
    }
    
    // executes the REST request using JQuery AJAX
    this.Execute = function() 
    {
        $.ajax(
        {       
            // We need a reference to the parent object due to scope resolution in nested functions such as the beforeSend function
            RESTCLientParent: this,
            type: this.RequestType,
            url: this.RequestPath,
            beforeSend: function(xhr)
            {
                // Set the Handler request parameter
                xhr.setRequestHeader(this.RESTCLientParent.RESTHandlerRequestKey,this.RESTCLientParent.RESTHandlerRequestParameter);
                xhr.RESTCLientParent = this.RESTCLientParent;
                
                $.each(Object.keys(this.RESTCLientParent.AdditionalParameters), function( index, value ) 
                {                    
                    xhr.setRequestHeader(value, xhr.RESTCLientParent.AdditionalParameters[value]);
                });               
            },
            success: this.SuccessHandler,
            error: this.ErrorHandler,
            data: (this.RequestType == "POST") ? this.FormParameters : undefined
        });
    }
    
    // Add an API parameter to the request headers
    this.AddAPIParameter = function (k, v)
    {
            this.AdditionalParameters.push({ key: k, value: v });
    }
    
    // Add a form key / value pair to the POST request form data
    this.AddFormParameter = function(k, v)
    {
        if(this.RequestType != "POST")
        {
            Logger.Log("AddFormParameter called to add parameter but request type is not POST");
        }
        this.FormParameters[k] = v;
    }
}
