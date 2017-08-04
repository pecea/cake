var liczba = 10;

new Job("t1").DependsOn("t2", "t3").Does(() => {
    liczba++;
	System.Console.WriteLine($"Zadanie t1: {liczba}");
});

new Job("t2").DependsOn("t3").Does(() => {
    liczba++;
	System.Console.WriteLine($"Zadanie t2: {liczba}");
});


new Job("t3").Does(() => {
    liczba++;
	System.Console.WriteLine($"Zadanie t3: {liczba}");
});
JobManager.SetDefault("t1");