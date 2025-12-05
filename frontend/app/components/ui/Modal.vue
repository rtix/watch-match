<template>
  <div v-if="hasTrigger" @click="toggleOpen">
    <slot name="trigger" />
  </div>

  <Teleport to="#teleports">
    <div v-if="isOpened" class="modal-overlay" @click="closeOnOverlayClick">
      <div ref="modal" class="modal" v-bind="$attrs">
        <slot>0_0</slot>
      </div>
    </div>
  </Teleport>
</template>

<script lang="ts" setup>
defineOptions({
  inheritAttrs: false,
});

const slots = useSlots();
const hasTrigger = Boolean(slots.trigger);
const isOpened = ref(!hasTrigger);

const emit = defineEmits(["close", "open"]);

const closeOnOverlayClick = ({ currentTarget, target }: MouseEvent) => {
  if (target === currentTarget) {
    toggleOpen();
  }
};

function toggleOpen() {
  if (isOpened.value) {
    emit("close", null);
  } else {
    emit("open", null);
  }
  isOpened.value = !isOpened.value;
}

onKeyStroke(
  "Escape",
  (e) => {
    if (!isOpened.value) {
      return;
    }

    e.preventDefault();
    toggleOpen();
  },
  { eventName: "keydown", dedupe: true }
);
</script>

<style scoped>
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100vw;
  height: 100dvh;

  background-color: hsla(0, 0%, 0%, 0.5);

  display: flex;
  justify-content: center;
  align-items: center;
}

.modal {
  position: relative;
  background-color: var(--color-background);
  padding: var(--spacing-md);
}
</style>
