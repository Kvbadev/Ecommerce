<script lang="ts">

</script>

<div class="container">
    <form>
        <div class="cardholder-name-div">
            <label for="cardholder-name">Cardholder Name</label>
            <input class="cardholder-name" type="text">
        </div>
        <div class="card-number-div">
            <label for="card-number">Credit card number</label>
            <input class="card-number" type="text">
        </div>
        <div class="email-div">
            <label for="email">Credit card number</label>
            <input class="email" type="text">
        </div>
        <div class="exp-cvv-div">
            <label for="expiration">Expiration</label>
            <input class="expiration" type="text">
            <label for="cvv">CVV</label>
            <input class="cvv" type="text">
        </div>
    </form>
</div>

<style>
    .container {
        width: auto;
    }
    form {
        display: flex;
        flex-direction: column;
        justify-content: center;
        width: 20%;
    }
    form * {
        margin: 0.3rem;
    }
    form div {
        display: flex;
        justify-content: space-between;
    }
</style>