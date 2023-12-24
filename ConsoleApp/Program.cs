using ConsoleApp;

new Tester(
    TimeSpan.FromSeconds(1),
    new Uri("https://google.com"),
    Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
        "nettest.log"))
.Start();
