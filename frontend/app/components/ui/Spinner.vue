<template>
  <div class="loader__outer">
    <span class="loader" />
  </div>
</template>

<script lang="ts" setup></script>

<style scoped>
.loader__outer {
  display: flex;
  justify-content: center;
  align-items: center;
}

.loader {
  width: calc(var(--spacing-lg) + 4px);
  height: calc(var(--spacing-lg) + 4px);
  border-radius: 50%;
  display: inline-block;
  position: relative;
  background: linear-gradient(
    0deg,
    hsla(var(--color-text--hsl), 0.2) 33%,
    hsla(var(--color-text--hsl)) 100%
  );
  box-sizing: border-box;
  animation: rotation 1s linear infinite;
}

.loader::after {
  content: "";
  position: absolute;
  left: 50%;
  top: 50%;
  transform: translate(-50%, -50%);
  border-radius: 50%;
  background: var(--color-background);
  width: var(--spacing-lg);
  height: var(--spacing-lg);
}

@keyframes rotation {
  0% {
    transform: rotate(0deg);
  }
  100% {
    transform: rotate(360deg);
  }
}
</style>
