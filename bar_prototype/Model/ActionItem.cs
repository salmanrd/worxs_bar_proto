﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace bar_prototype.Model
{
    public enum StatusType
    {
        Pending,
        Submitted,
        PendingApproval,
        Approved,
        Rejected,
        TransferredToBar
    }

    public abstract class ActionItem
    {
        protected ActionItem()
        {
            PaymentGroup = new PaymentGroup();
            Fees = new List<Fee>();
            Remissions = new List<Remission>();
        }

        public abstract string ActionType { get; }
        public int Id { get; set; }

        public PaymentGroup PaymentGroup { get; set; }
        public StatusType Status { get; set; }

        public List<Remission> Remissions { get; set; }
        public List<Fee> Fees { get; set; }

        public double CalculateUnallocatedPayment()
        {
            var totalFees = Fees.Sum(x => x.CalculatedAmount);
            var totalRemission = Remissions.Sum(x => x.Amount);
            var totalPayment = PaymentGroup.CalculateTotalPayment();

            return totalPayment + totalRemission - totalFees;
        }
    }



}