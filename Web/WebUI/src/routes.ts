import Products from "./lib/Components/Products/Homepage/Products.svelte";
import NotFound from "./lib/Components/Errors/NotFound.svelte";
import ProductDetails from "./lib/Components/Products/Details/ProductDetails.svelte";
import LoginForm from "./lib/Components/Account/LogIn/LoginForm.svelte";
import SignUpForm from "./lib/Components/Account/SignUp/SignUpForm.svelte";
import ProfilePage from "./lib/Components/Account/Profile/ProfilePage.svelte";
import ShoppingCart from "./lib/Components/ShoppingCart/ShoppingCart.svelte";
import BuyProducts from "./lib/Components/BuyProducts/BuyProducts.svelte";

export const routes: any = {
    '/': Products,
    '/product/:id': ProductDetails,
    '/account/signup': SignUpForm,
    '/account/login': LoginForm,
    '/profile': ProfilePage,
    '/ShoppingCart': ShoppingCart,
    '/Buy': BuyProducts,
    '*': NotFound
}