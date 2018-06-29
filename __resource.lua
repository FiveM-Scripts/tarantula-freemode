resource_manifest_version "05cfa83c-a124-4cfa-a768-c24a5811d8f9"

files {
	"Newtonsoft.Json.dll"
}

shared_script "freeroamshared.net.dll"
client_scripts {
	"nativeui.net.dll",
	"freeroam.net.dll"
}
server_script "freeroamserver.net.dll"