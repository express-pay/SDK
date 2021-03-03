# ExpressPay.SDK
Официальное SDK сервиса Экспресс Платежи для языка C# 

Пример использования

```c#
var sdk = new ExpressPaySdk(true, "a75b74cbcfe446509e8ee874f421bd64");
Console.WriteLine(await sdk.GetInvoices());