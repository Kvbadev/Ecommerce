<script lang="ts">
//TODO: Add functionality to those buttons
</script>

<div class="container">
    
    <button class="buy-btn"><span>Buy Now</span></button>
    <button class="cart-btn"><span>Add to cart</span></button>
</div>

<style>
    button {
        cursor: pointer;
        font-family: Raleway;
        border: black 0.4rem solid;
        width: 33rem;
        height: 9rem;
        background-color: transparent;
        font-size: 3rem;
        text-align: center;
        transition: 0.5s ease-in-out;
    }
    span::after {
        content: '»'; 
        position: absolute;
        right: -2rem;
        top: 0;
        /* left: 1.5rem; */
        opacity: 0;
        transition: 0.5s;
    }
    button:hover span:after {
        opacity: 1;
        right: 0;
    }
    span {
        display: inline-block;
        position: relative;
        transition: 0.5s; 
    }
    button:hover span{
        padding-right: 2.5rem;
    }
    div {
        display: flex;
        align-items: center;
        justify-content: space-around;
    }
    .cart-btn {
        background-color: black;
    }
    .cart-btn span{
        color: white;
    }

</style>