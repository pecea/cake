#r "C:/Users/ErnestPrzestrzelski/Desktop/Praca/Cake/Minify/bin/Debug/Minify.dll"

 new Job("BundleHtml").Does(() => {
     return Minify.Methods.BundleFiles(@"D:\Dane\Ernest\Praca\cake\Cake\scripts\*.html", 
         @"D:\Dane\Ernest\Praca\TestOutput\bundled.html", '\n');
 });

 JobManager.SetDefault("BundleHtml");
