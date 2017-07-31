public class MyClass {
	public int PropertyX {get;set;}
	
	public MyClass(){
		PropertyX = 10;
	}

}




var a = new MyClass();

System.Console.WriteLine(a.PropertyX);