<?php
require_once('EasyGamecredits.php');
$method = $_POST["method"];
$params = $_POST["params"];
$userName = $_POST["rpcusername"];
$password = $_POST["rpcpassword"];
$Port = $_POST["port"];
$QTCoreWallets = new Gamecredits($userName,$password,'127.0.0.1',$Port);
//$AllowedCommands = array("getinfo","getnetworkinfo","listtransactions","gettransaction","getnewaddress");
//You can add or comment any commands you feel safe. I personally think you don't need anymore than these in order to except crypto payment.
switch($method)
{
	case 'getreceivedbyaddress':
		$CallCommand=$QTCoreWallets->getreceivedbyaddress($params);
		echo json_encode(array('result'=>'success','command'=>$method,'params'=>$params,'data'=>$CallCommand), JSON_UNESCAPED_UNICODE);
		break;
	case 'getbalance': //You should require a param with this function otherwise you could expose your whole wallet balance.
		$CallCommand=$QTCoreWallets->getbalance($params);
		echo json_encode(array('result'=>'success','command'=>$method,'params'=>$params,'data'=>$CallCommand), JSON_UNESCAPED_UNICODE);
		break;
	case 'help':
		$CallCommand=$QTCoreWallets->help($params);
		echo json_encode(array('result'=>'success','command'=>$method,'params'=>$params,'data'=>$CallCommand), JSON_UNESCAPED_UNICODE);
		break;
	case 'ping':
		$CallCommand=$QTCoreWallets->ping();
		echo json_encode(array('result'=>'success','command'=>$method,'params'=>$params,'data'=>$CallCommand), JSON_UNESCAPED_UNICODE);
		break;
	case 'getinfo': //NOT SAVE BECAUSE IT SHOWS THE BALANCE OF YOUR WALLET
		$CallCommand=$QTCoreWallets->getinfo();
		echo json_encode(array('result'=>'success','command'=>$method,'params'=>$params,'data'=>$CallCommand), JSON_UNESCAPED_UNICODE);
		break;
	case 'getnetworkinfo': //NOT SAFE BECAUSE IT SHOWS THE NETWORK INFORMATION OF YOUR WALLET
		$CallCommand=$QTCoreWallets->getnetworkinfo();
		echo json_encode(array('result'=>'success','command'=>$method,'params'=>$params,'data'=>$CallCommand,'Error'=>null), JSON_UNESCAPED_UNICODE);
		break;
	case 'listtransactions':
		$CallCommand=$QTCoreWallets->listtransactions();
		echo json_encode(array('result'=>'success','command'=>$method,'params'=>$params,'data'=>$CallCommand,'Error'=>null), JSON_UNESCAPED_UNICODE);
		break;
	case 'gettransaction':
		$CallCommand=$QTCoreWallets->gettransaction($params);
		echo json_encode(array('result'=>'success','command'=>$method,'params'=>$params,'data'=>$CallCommand,'Error'=>null), JSON_UNESCAPED_UNICODE);
		break;
	case 'getnewaddress':
		$CallCommand=$QTCoreWallets->getnewaddress($params);
		echo json_encode(array('result'=>'success','command'=>$method,'params'=>$params,'data'=>$CallCommand,'Error'=>null), JSON_UNESCAPED_UNICODE);
		break;
	default:
		echo json_encode(array('result'=>'Failed','command'=>$method,'params'=>$params,'data'=>$CallCommand,'Error'=>"No Valid command recieved"), JSON_UNESCAPED_UNICODE);
		break;
}
?>
