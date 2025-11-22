<template>
  <div v-if="hasTrigger" @click="toggleOpen">
    <slot name="trigger"/>
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

const emit = defineEmits(["close"]);

const closeOnOverlayClick = ({ currentTarget, target }: MouseEvent) => {
  if (target === currentTarget) {
    toggleOpen();
  }
};

function toggleOpen() {
  isOpened.value = !isOpened.value;
  if (isOpened.value) {
    emit("close", null);
  }
}

onKeyStroke(
  "Escape",
  e => {
    if (!isOpened.value) {
      return;
    }

    e.preventDefault();
    toggleOpen();
  },
  { eventName: "keydown", dedupe: true },
);
</script>

<style scoped>
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100vw;
  height: 100dvh;

  background-color: hsla(var(--background-color--hsl), 0.5);

  display: flex;
  justify-content: center;
  align-items: center;
}

.modal {
  position: relative;
}
</style>
