# PlainMarkupLanguage

I'm implementing a recursive parser for the same markup language in different languages to familiarize myself with those languages.

Here's a sample of the ML

```
﻿firstName John
lastName Smith
isAlive true
age 27
address
	streetAddress 21 2nd Street
	city New York
	state NY
	postalCode 10021-3100
phoneNumbers
	type home
	number 212 555-1234

	type office
	number 646 555-4567

	type mobile
	number 123 456-7890
children
spouse null
```

Which is equivalent to the Wikipedia JSON example:

```json
{
	"firstName": "John",
	"lastName": "Smith",
	"isAlive": true,
	"age": 27,
	"address": {
		"streetAddress": "21 2nd Street",
		"city": "New York",
		"state": "NY",
		"postalCode": "10021-3100"
	},
	"phoneNumbers": [
		{
			"type": "home",
			"number": "212 555-1234"
		},
		{
			"type": "office",
			"number": "646 555-4567"
		},
		{
			"type": "mobile",
			"number": "123 456-7890"
		}
	],
	"children": [],
	"spouse": null
}
```

## This parser sucks

because it's intended to test functionalities in languages and not to parse things well.

## License

Feel free to steal it.
