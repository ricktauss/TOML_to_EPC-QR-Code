Create TOML files here

Template:

#TOML file for configuring Payments for which QR-Codes will be generated

[receiver]
name = "Receiver name or institution"
iban = "AT12 1234 1234 1234 1234"
bic = "RLBBAT2E027"


# Payment reference must be lower or equal 35 characters and doesn't allow umlauts (üöäÜÖÄ)

[[payments]]
reference = "Reference text 1"
amount = 10.00

[[payments]]
reference = "Reference text 2"
amount = 20.00

[[payments]]
reference = "Reference text 3"
amount = 15.00

[[payments]]
reference = "Reference text 4"
amount = 30.00