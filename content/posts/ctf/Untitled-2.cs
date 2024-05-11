int64_t method.crab_rave::litcrypt_internal::decrypt_bytes.h32388621a43d3c14
                  (int64_t arg1, int64_t arg2, int64_t arg3, int64_t arg4, int64_t arg_28h, int64_t arg_60h)
{
    uint8_t *puVar1;
    uint32_t *puVar2;
    uint32_t *puVar3;
    uint8_t uVar4;
    uint8_t uVar5;
    uint32_t uVar6;
    uint32_t uVar7;
    uint32_t uVar8;
    uint32_t uVar9;
    uint32_t uVar10;
    uint32_t uVar11;
    uint32_t uVar12;
    int64_t iVar13;
    uint64_t uVar14;
    uint64_t uVar15;
    int64_t iVar16;
    uint64_t uVar17;
    uint8_t *puVar18;
    uint32_t uVar19;
    undefined auVar20 [16];
    int64_t var_a8h;
    int64_t var_98h;
    int64_t var_90h;
    int64_t var_88h;
    int64_t var_80h;
    LPDWORD var_78h;
    int64_t var_70h;
    int64_t var_68h;
    int64_t var_60h;
    int64_t var_58h;
    int64_t var_50h;
    int64_t var_48h;
    int64_t var_40h;
    
    if (arg_28h != 1) {
        var_78h = (LPDWORD)(arg3 + arg2);
        _var_68h = (undefined  [16])0x0;
        var_58h = 0;
        var_48h = arg_28h;
        var_40h = 0;
        var_70h = arg2;
        var_50h = arg4;
        _ZN98_$LT$alloc..vec..Vec$LT$T$GT$$u20$as$u20$alloc..vec..spec_from_iter..SpecFromIter$LT$T$C$I$GT$$GT$9from_iter17hdde5d9baf9b
                  ((int64_t)&var_90h, (int64_t)&var_78h);
        iVar13 = var_88h;
        arg3 = var_90h;
        goto code_r0x1000cb2b;
    }
    puVar1 = (uint8_t *)(arg2 + arg3);
    if (arg3 == 0) {
        var_90h = 0;
        var_88h = 1;
        iVar13 = 1;
        var_80h = arg3;
        goto code_r0x1000cb2b;
    }
    if (arg3 < 0) goto code_r0x1000cbee;
    uVar4 = *(uint8_t *)arg4;
    uVar14 = (uint64_t)(arg3 >= 0);
    iVar13 = __rust_alloc(arg3, uVar14);
    if (iVar13 == 0) goto code_r0x1000c94d;
    var_88h = iVar13;
    if ((uint64_t)arg3 < 8) {
        uVar14 = 0;
    } else {
        if ((uint64_t)arg3 < 0x20) {
            uVar17 = 0;
        } else {
            uVar14 = arg3 & 0xffffffffffffffe0;
            auVar20 = pshuflw(ZEXT216(CONCAT11(uVar4, uVar4)), ZEXT216(CONCAT11(uVar4, uVar4)), 0);
            uVar19 = auVar20._0_4_;
            uVar17 = (uVar14 - 0x20 >> 5) + 1;
            if (uVar14 - 0x20 == 0) {
                iVar16 = 0;
            } else {
                uVar15 = uVar17 & 0xfffffffffffffffe;
                iVar16 = 0;
                do {
                    puVar2 = (uint32_t *)(arg2 + iVar16);
                    uVar10 = puVar2[1];
                    uVar11 = puVar2[2];
                    uVar12 = puVar2[3];
                    puVar3 = (uint32_t *)(arg2 + 0x10 + iVar16);
                    uVar6 = *puVar3;
                    uVar7 = puVar3[1];
                    uVar8 = puVar3[2];
                    uVar9 = puVar3[3];
                    puVar3 = (uint32_t *)(iVar13 + iVar16);
                    *puVar3 = *puVar2 ^ uVar19;
                    puVar3[1] = uVar10 ^ uVar19;
                    puVar3[2] = uVar11 ^ uVar19;
                    puVar3[3] = uVar12 ^ uVar19;
                    puVar2 = (uint32_t *)(iVar13 + 0x10 + iVar16);
                    *puVar2 = uVar6 ^ uVar19;
                    puVar2[1] = uVar7 ^ uVar19;
                    puVar2[2] = uVar8 ^ uVar19;
                    puVar2[3] = uVar9 ^ uVar19;
                    puVar2 = (uint32_t *)(arg2 + 0x20 + iVar16);
                    uVar10 = puVar2[1];
                    uVar11 = puVar2[2];
                    uVar12 = puVar2[3];
                    puVar3 = (uint32_t *)(arg2 + 0x30 + iVar16);
                    uVar6 = *puVar3;
                    uVar7 = puVar3[1];
                    uVar8 = puVar3[2];
                    uVar9 = puVar3[3];
                    puVar3 = (uint32_t *)(iVar13 + 0x20 + iVar16);
                    *puVar3 = *puVar2 ^ uVar19;
                    puVar3[1] = uVar10 ^ uVar19;
                    puVar3[2] = uVar11 ^ uVar19;
                    puVar3[3] = uVar12 ^ uVar19;
                    puVar2 = (uint32_t *)(iVar13 + 0x30 + iVar16);
                    *puVar2 = uVar6 ^ uVar19;
                    puVar2[1] = uVar7 ^ uVar19;
                    puVar2[2] = uVar8 ^ uVar19;
                    puVar2[3] = uVar9 ^ uVar19;
                    iVar16 = iVar16 + 0x40;
                    uVar15 = uVar15 - 2;
                } while (uVar15 != 0);
            }
            if ((uVar17 & 1) != 0) {
                puVar2 = (uint32_t *)(arg2 + iVar16);
                uVar10 = puVar2[1];
                uVar11 = puVar2[2];
                uVar12 = puVar2[3];
                puVar3 = (uint32_t *)(arg2 + 0x10 + iVar16);
                uVar6 = *puVar3;
                uVar7 = puVar3[1];
                uVar8 = puVar3[2];
                uVar9 = puVar3[3];
                puVar3 = (uint32_t *)(iVar13 + iVar16);
                *puVar3 = *puVar2 ^ uVar19;
                puVar3[1] = uVar10 ^ uVar19;
                puVar3[2] = uVar11 ^ uVar19;
                puVar3[3] = uVar12 ^ uVar19;
                puVar2 = (uint32_t *)(iVar13 + 0x10 + iVar16);
                *puVar2 = uVar6 ^ uVar19;
                puVar2[1] = uVar7 ^ uVar19;
                puVar2[2] = uVar8 ^ uVar19;
                puVar2[3] = uVar9 ^ uVar19;
            }
            var_90h = arg3;
            var_80h = arg3;
            if (uVar14 == arg3) goto code_r0x1000cb2b;
            uVar17 = uVar14;
            if ((arg3 & 0x18U) == 0) {
                arg2 = arg2 + uVar14;
                goto code_r0x1000cafc;
            }
        }
        uVar14 = arg3 & 0xfffffffffffffff8;
        auVar20 = pshuflw(ZEXT216(CONCAT11(uVar4, uVar4)), ZEXT216(CONCAT11(uVar4, uVar4)), 0);
        do {
            *(uint64_t *)(iVar13 + uVar17) = *(uint64_t *)(arg2 + uVar17) ^ auVar20._0_8_;
            uVar17 = uVar17 + 8;
        } while (uVar14 != uVar17);
        arg2 = (int64_t)(arg2 + uVar14);
        var_90h = arg3;
        var_80h = arg3;
        if (uVar14 == arg3) goto code_r0x1000cb2b;
    }
code_r0x1000cafc:
    puVar18 = (uint8_t *)(iVar13 + uVar14);
    do {
        uVar5 = *(uint8_t *)arg2;
        arg2 = arg2 + 1;
        *puVar18 = uVar5 ^ uVar4;
        puVar18 = puVar18 + 1;
        var_90h = arg3;
        var_80h = arg3;
    } while ((uint8_t *)arg2 != puVar1);
code_r0x1000cb2b:
    iVar16 = var_80h;
    dbg.from_utf8(&var_78h, iVar13, var_80h);
    if ((var_78h != (LPDWORD)0x0) && ((char)var_68h != '\x02')) {
        var_78h = (LPDWORD)var_70h;
        var_70h = SUB168(_var_68h, 0);
        var_60h = iVar13;
        var_68h = arg3;
        var_58h = iVar16;
        dbg.unwrap_failed(data.103cdcd0, 0x2b, &var_78h, data.103cdd00, data.103cdd98);
        do {
            invalidInstructionException();
        } while( true );
    }
    *(int64_t *)arg1 = arg3;
    *(int64_t *)(arg1 + 8) = iVar13;
    *(int64_t *)(arg1 + 0x10) = iVar16;
    return arg1;
}
