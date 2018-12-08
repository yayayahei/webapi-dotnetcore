$server=''
$pass=ConvertTo-SecureString -String '' -AsPlainText -Force
$cre=New-Object pscredential('', $pass)
$session=New-PSSession -ComputerName $server -Credential $cre


ls test | cp -Destination "c:\\" -ToSession $session -Recurse -Force