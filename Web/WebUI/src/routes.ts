import Products from "./lib/Components/Homepage/Products/Products.svelte";
import NotFound from "./lib/Components/Errors/NotFound.svelte";

export const routes: any = {
    '/': Products,
    '*': NotFound
}