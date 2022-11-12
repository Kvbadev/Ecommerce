<script lang="ts">

import { userProfile, shoppingCart} from '../../Stores/stores';
import {link} from 'svelte-spa-router'
import Logo from "./Logo.svelte";
import Fa from 'svelte-fa';
import { faShoppingCart } from '@fortawesome/free-solid-svg-icons';
import { fade } from 'svelte/transition';
let cartColor = 'white';
</script>

    <div class="group-1 group">
        <li class="nav-item item-1">
            <a href="/About" use:link>About Us</a>
        </li>
        <li class="nav-item item-2">
            <a href="/Contact" use:link>Contact</a>
        </li>
    </div>
    <Logo />
    <div class="group-2 group">
        <li class="nav-item item-3">
            <a href="/ShoppingCart" use:link class="cart-link">
                {#if $shoppingCart}
                <div class="products-count"><span transition:fade>
                    {$shoppingCart.count}
                </span></div>
                {/if}
                <Fa icon={faShoppingCart} size="lg" color={cartColor}/>
            </a>
        </li>
        <li class="nav-item item-4">
            {#if $userProfile}
            <a href="/Profile" use:link>{$userProfile.username}</a>
            {:else}
            <a href="/Account/Login" use:link>Log In </a>
            {/if}
        </li>
    </div>
    
<style>
    @media only screen and (min-width: 320px) {
        .group {
            display: none;
        }
    }
    @media only screen and (min-width: 768px) {
        .group {
            display: none;
        }
    }
    @media only screen and (min-width: 992px) {
        .group {
            display: flex;
            flex-direction: row;
            width: 25%;
            align-items: center;
            margin: 0 1rem;
        }
        .group-1 {
            justify-content: flex-start;
        }
        .group-2 {
            justify-content: flex-end;
        }
        .cart-link {
            display: flex;
            flex-direction: row;
            align-items: center;
            justify-content: space-between;
            width: 7.5rem;
        }
        .products-count{
            display: flex;
            bottom: 35%;
            left: 40%;
            justify-content: center;
            align-items: center;
            border: 0.2rem white solid;
            border-radius: 5rem;
        }
        .products-count span {
            font-size: 2rem;
            color: white;
            height: auto;
            padding: 0.7rem;
        }
        li {
            flex: 0 1 auto;
            width: 30%;
            text-align: center;
            font-size: 1.7vw;
            cursor: pointer;
            margin: 0 2rem;
        }

        a {
            color: white;
            transition: color 0.5s;
        }
        .item-4 {
            width: auto;
        }
        .item-3 {
            width: auto;
        }
        a:hover {
            color: rgb(216, 211, 211);
        }
        .group-2:last-child {
            color: #7e7603;
        }
    }
</style>
    