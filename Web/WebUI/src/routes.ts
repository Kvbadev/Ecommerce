import Products from "./lib/Components/Products/Homepage/Products.svelte";
import NotFound from "./lib/Components/Errors/NotFound.svelte";
import ProductDetails from "./lib/Components/Products/Details/ProductDetails.svelte";
import LoginForm from "./lib/Components/Account/LogIn/LoginForm.svelte";

export const routes: any = {
    '/': Products,
    '/product/:id': ProductDetails,
    '/account/login': LoginForm,
    '*': NotFound
}