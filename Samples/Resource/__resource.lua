resource_type 'gametype' { name = 'Puma' }
description 'PumaSamples'

client_scripts {
	'Client.net.dll',
}

server_scripts {
	'Server.net.dll',
}

files {
	'PumaCore.dll',
	'PumaClient.dll',
	'System.ValueTuple.dll',
	'Microsoft.Experimental.Collections.dll',
}
