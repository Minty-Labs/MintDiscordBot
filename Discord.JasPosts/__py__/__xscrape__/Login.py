import argparse
import asyncio
from twscrape import API

async def main(username, password, email, email_password, cookies=None):
    api = API()  # or API("path-to.db") - default is `accounts.db`
    # ADD ACCOUNTS
    if cookies:
        await api.pool.add_account(username, password, email, email_password, cookies=cookies)
    else:
        await api.pool.add_account(username, password, email, email_password)
    await api.pool.login_all()

if __name__ == "__main__":
    parser = argparse.ArgumentParser(description="Login script with parameters.")
    parser.add_argument("username", help="Your Twitter Account Username")
    parser.add_argument("password", help="Your Twitter Account Password")
    parser.add_argument("email", help="Email associated with the Twitter account")
    parser.add_argument("email_password", help="Password required to receive the verification code via IMAP protocol")
    parser.add_argument("cookies", nargs="?", default=None, help="Optional cookie data")
    args = parser.parse_args()
    asyncio.run(main(args.username, args.password, args.email, args.email_password, args.cookies))
