
$scriptPath = Split-Path -Parent $MyInvocation.MyCommand.Path
$protoPath = ".\"
$srcPath = "..\"

$array = Get-ChildItem -Path $protoPath -Name "*.proto" 
for($i = 0; $i -lt $array.Count; $i++){
    # $array[$i] = $protoPath + "\" + $array[$i];
    echo $array[$i]
    ./protoc --proto_path=$protoPath --csharp_out=$srcPath $array[$i]
} 
