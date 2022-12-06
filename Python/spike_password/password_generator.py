import string
from sys import argv
from secrets import choice

def get_generator_args():
    args = argv[1:]
    if len(args) != 4:
        return 8, True, True, False
    try:
        result = [int(args[0])]
        for a in args[1:]:
            result += [a in ['True', 'true', 'T', 't', 1]]
        return tuple(result)
    except:
        return 8, True, True, False

def get_allowed_characters(use_letters, use_digits, use_punctuation):
    allowed_characters = ''
    if use_letters:
        allowed_characters += string.ascii_letters
    if use_digits:
        allowed_characters += string.digits
    if use_punctuation:
        allowed_characters += string.punctuation
    return allowed_characters

def check_condition(password, use_letters, use_digits, use_punctuation):
    conditions = True
    if (use_letters): 
        conditions = conditions and any(c.islower() for c in password) and any(c.isupper() for c in password)
    if (use_digits):
        conditions = conditions and any(c.isdigit() for c in password)
    if (use_punctuation):
        conditions = conditions and any(c in string.punctuation for c in password)
    return conditions

def generate_password(max_len, use_letters, use_digits, use_punctuation):
    characters = get_allowed_characters(use_letters, use_digits, use_punctuation)
    password = ''
    while not check_condition(password, use_letters, use_digits, use_punctuation):
        password = ''.join(choice(characters) for i in range(max_len))
    return password

lenght, useL, useD, useP = get_generator_args()
password = generate_password(lenght, useL, useD, useP)

print(password)