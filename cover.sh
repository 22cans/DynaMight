#testCommand=$( dotnet test --collect:"XPlat Code Coverage" )
testCommand=$( dotnet test -s CodeCoverage.runsettings )

line=$( echo "$testCommand" | grep -n "Attachments"  | sed 's/:.*//' )
line=`expr "$line" + 1`
report=$( echo "$testCommand" | head -$line | tail -1 | awk '{$1=$1;print}' )

reportgenerator -reports:"$report" -targetdir:"coveragereport" -reporttypes:Html

rm -rf tests/Legacy.Application.Tests/TestResults

start ./coveragereport/index.html
open ./coveragereport/index.html
