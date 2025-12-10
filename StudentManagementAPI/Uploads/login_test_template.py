from playwright.sync_api import Page, expect, BrowserContext
from Test import browser
from Test.CIServerXTest import CIServerXTest
from Tests import *

class CSLoginTestTemplate(CIServerXTest):

    event = None
    event_till_status = None
    k37a_till_status = None
    event_total_inventory = dict()
    k37a_total_inventory = dict()
    event_sales_till = dict()
    k37a_sales_till = dict()
    event_sales_user = dict()
    k37a_sales_user = dict()
    event_sales_back_machine = dict()
    k37a_sales_back_machine = dict()
    event_history = dict()
    k37a_history = dict()

    def setup_class(self):
        CIServerXTest.setup_class(self)
        self.username = "1"
        self.password = "1111"

    def login(self, browser: BrowserContext, request = None) -> Page:

        try:
            self.file_path = str(request.fspath)

            page = browser.new_page()

            self.page = page

            page.goto(self.base_url + "/ci/Ci03040.do")

            user_input = page.locator(".userId")

            user_input.click()

            user_input.fill(self.username)

            password_input = page.locator(".password")

            password_input.click()

            password_input.fill(self.password)

            take_screenshot(test_file_path=self.file_path, filename="K07 Login Page")

            page.get_by_role("button", name="Login").click()
            expect(page.locator("#_storeNameHeader_")).to_contain_text("K07")

            expect(page.get_by_role("heading", name="Inventory Management")).to_be_visible()
            take_screenshot(test_file_path=self.file_path, filename="K07 Dashboard Page")
                
        except Exception as e:

            take_screenshot(test_file_path=self.file_path, filename="K07 Login Page not found")


    def logout(self):
        expect(self.page.locator(".headerlink")).to_be_visible()
        self.page.locator(".headerlink").click()


