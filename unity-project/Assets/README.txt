# Unity Flux Plugin

# Build process
	1. Build the webgl client
	2. Paste the following lines into the <head>:
	    <script src="../flux-sdk.js"></script>
		<script src="../flux-sdk-login.js"></script>
    3. Serve it using the following command:
    	python -m SimpleHTTPServer 8091	