dotnet test --logger "trx;LogFileName=TestResults.trx"
trxlog2html -i "QLAB_2.Tests/TestResults/TestResults.trx" -o "QLAB_2.Tests/TestResults/TestReport.html"
Start-Process "QLAB_2.Tests/TestResults/TestReport.html"
