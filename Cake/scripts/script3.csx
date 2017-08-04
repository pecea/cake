
public static void PrintX(int x) {System.Console.WriteLine(x);}

public class MyClass {
	public int PropertyX = 10;
	
	public void PrintNumber() {PrintX(PropertyX);}
	
	public void Print() {System.Console.WriteLine(PropertyX);}
	
	public int AccessX() {return PropertyX;}

}