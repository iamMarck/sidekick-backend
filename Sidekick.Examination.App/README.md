# SideKick Examination App

#### App Specs
#####App is created on this ff technology
- Microsoft .NET CORE 5.0
- GOOGLE Angular TS 8
- Websockets are used for backend communication

##### Authentication/Security
- SHA-256 Hashing are used for encryptions and keyword combinations and salt are added.


#### Implemented parts:
- I was able to impelement websocket commands

	1. Login
	2. Registration
	3. Validation of username and email existence
	4. Sending of verification code thru email
		

	
- I has able to encryp/hassed keys that was need to be able to authenticate the user account
			//Hashing, BlobToString, Hash_Hmac can be located in:
			websocket.service.ts

###Fix/Adjustment on given notes 
- Responsive design is implemented on this App.
- Added  [green check] badge image right after label "Username" and "Email"  from registration form. This badge shows if the given entry is already registered on the App; and validation message "given user/email already exists" will show below the input box if already registered.

- I made adjustment on registration procedure verify code is now seperately entered after user gives required details the app will send the verification code based on users validated email then the seperate entry on verification code will cover the registration form.

- Simple shared component is created 
			//can be located on: 
			app/shared/validity/validity.component.html
			

- Logout is added to go back to login page and clear the logged user session.
- Revamp on overall design are made.
- README is now changed to .md file
- Angular Project is not moved to seperate project to showcase knowledge on .Net as i am also knowledgeable on doing backend and .net tasks aside from angular ts and web designing.


#THANK YOU VERY MUCH

Submitted by: [MARCK F. ANTONIO](https://www.linkedin.com/in/marck-antonio/ "MARCK F. ANTONIO")
