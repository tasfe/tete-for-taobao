/**
 * This is a generated class and is not intended for modfication.  To customize behavior
 * of this service wrapper you may modify the generated sub-class of this class - Getproduct.as.
 */
package services.getproduct
{
import com.adobe.fiber.core.model_internal;
import com.adobe.fiber.services.wrapper.HTTPServiceWrapper;
import mx.rpc.AbstractOperation;
import mx.rpc.AsyncToken;
import mx.rpc.http.HTTPMultiService;
import mx.rpc.http.Operation;

import com.adobe.serializers.xml.XMLSerializationFilter;

[ExcludeClass]
internal class _Super_Getproduct extends com.adobe.fiber.services.wrapper.HTTPServiceWrapper
{      
    private static var serializer0:XMLSerializationFilter = new XMLSerializationFilter();
       
    // Constructor
    public function _Super_Getproduct()
    {
        // initialize service control
        _serviceControl = new mx.rpc.http.HTTPMultiService("http://groupbuy.7fshop.com/");
         var operations:Array = new Array();
         var operation:mx.rpc.http.Operation;
         var argsArray:Array;       
         
         operation = new mx.rpc.http.Operation(null, "Operation1");
         operation.url = "testflash/data.aspx";
         operation.method = "GET";
         argsArray = new Array("a","b");
         operation.argumentNames = argsArray;         
         operation.serializationFilter = serializer0;
		 operation.resultType = String; 		 
         operations.push(operation);
    
         _serviceControl.operationList = operations;  

    
                      
         model_internal::initialize();
    }

	/**
	  * This method is a generated wrapper used to call the 'Operation1' operation. It returns an mx.rpc.AsyncToken whose 
	  * result property will be populated with the result of the operation when the server response is received. 
	  * To use this result from MXML code, define a CallResponder component and assign its token property to this method's return value. 
	  * You can then bind to CallResponder.lastResult or listen for the CallResponder.result or fault events.
      *
      * @see mx.rpc.AsyncToken
      * @see mx.rpc.CallResponder 
      *
      * @return an mx.rpc.AsyncToken whose result property will be populated with the result of the operation when the server response is received.
	  */          
	public function Operation1(a:String, b:String) : mx.rpc.AsyncToken
	{
		var _internal_operation:mx.rpc.AbstractOperation = _serviceControl.getOperation("Operation1");
		var _internal_token:mx.rpc.AsyncToken = _internal_operation.send(a,b) ;

		return _internal_token;
	}   
	 
}

}
